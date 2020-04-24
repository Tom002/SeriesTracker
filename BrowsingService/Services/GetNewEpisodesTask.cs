using AutoMapper;
using BrowsingService.Data;
using BrowsingService.Models;
using Common.Events;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BrowsingService.Services
{
    internal interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
    public class GetNewEpisodesTask : IScopedProcessingService
    {
        private readonly IMovieDbClient _apiClient;

        private readonly BrowsingDbContext _context;

        private readonly ICapPublisher _capBus;

        private const int _refreshIntervalSeconds = 3600;

        private readonly IMapper _mapper;

        const string imageBasePath = "https://image.tmdb.org/t/p/original";
        public GetNewEpisodesTask(
            IMovieDbClient apiClient,
            BrowsingDbContext context,
            ICapPublisher capBus,
            IMapper mapper)
        {
            _apiClient = apiClient;
            _context = context;
            _capBus = capBus;
            _mapper = mapper;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Minden meglévő sorozathoz lekérdezem hogy jötttek-e ki új részek
                var seriesList = await _context.Series
                    .Include(s => s.Episodes)
                    .ToListAsync();

                var newEpisodeEventList = new List<EpisodeCreatedEvent>();

                foreach (var series in seriesList)
                {
                    var apiResponseSeries = await _apiClient.GetSeries(series.SeriesId);

                    if (
                        (!series.LastAirDate.HasValue && apiResponseSeries.last_air_date.HasValue)
                            ||
                        (series.LastAirDate.HasValue && series.LastAirDate < apiResponseSeries.last_air_date)
                      )
                    {
                        // Ha jött ki új rész az a legújabb évadban lesz
                        var lastSeasonNumber = apiResponseSeries.seasons.Max(s => s.season_number);
                        var lastSeason = await _apiClient.GetSeasonEpisodes(series.SeriesId, lastSeasonNumber);

                        if(!series.LastAirDate.HasValue)
                        {
                            // Ha 
                            series.LastAirDate = DateTime.MinValue;
                        }

                        
                        foreach (var apiEpisode in lastSeason.episodes)
                        {
                            if (!series.Episodes.Any(e => e.EpisodeId == apiEpisode.id))
                            {
                                var episode = new Episode
                                {
                                    EpisodeId = apiEpisode.id,
                                    Description = apiEpisode.overview.Substring(0, Math.Min(1500, apiEpisode.overview.Length)),
                                    EpisodeTitle = apiEpisode.name.Substring(0, Math.Min(200, apiEpisode.name.Length)),
                                    Release = apiEpisode.air_date,
                                    EpisodeNumber = apiEpisode.episode_number,
                                    Season = apiEpisode.season_number,
                                    SeriesId = series.SeriesId,
                                    CoverImageUrl = imageBasePath + apiEpisode.still_path
                                };

                                if(episode.Release.HasValue && episode.Release.Value > series.LastAirDate.Value)
                                {
                                    series.LastAirDate = episode.Release;
                                }

                                series.Episodes.Add(episode);

                                var episodeEvent = _mapper.Map<EpisodeCreatedEvent>(episode);
                                newEpisodeEventList.Add(episodeEvent);
                            }
                        }

                        if(series.LastAirDate == DateTime.MinValue)
                        {
                            series.LastAirDate = null;
                        }
                    }
                }

                using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: false))
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                        foreach (var episodeEvent in newEpisodeEventList)
                        {
                            await _capBus.PublishAsync("browsingservice.episode.created", episodeEvent);
                        }
                        await trans.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await trans.RollbackAsync();
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalSeconds), stoppingToken);
            }


            
        }
    }
}
