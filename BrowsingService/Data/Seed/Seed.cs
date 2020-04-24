using AutoMapper;
using BrowsingService.Models;
using BrowsingService.Services.ApiResponseModels;
using Common.Events;
using DotNetCore.CAP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Data
{
    public class Seed
    {
        public static async Task SeedData(BrowsingDbContext context, ICapPublisher capBus, IMapper mapper)
        {
            const string imageBasePath = "https://image.tmdb.org/t/p/original";

            List<CompleteSeries> seriesList = new List<CompleteSeries>();

            List<Series> seriesToCreate = new List<Series>();

            List<Artist> artistsToCreate = new List<Artist>();

            List<Category> categoriesToCreate = new List<Category>();

            seriesList = JsonConvert.DeserializeObject<List<CompleteSeries>>(File.ReadAllText(@"/app/Data/Seed/series.json"));

            foreach (var series in seriesList)            
            {
                var newSeries = new Series
                {
                    SeriesId = series.id,
                    Description = series.overview.Substring(0, Math.Min(2000, series.overview.Length)),
                    StartYear = series.first_air_date.HasValue ? series.first_air_date.Value.Year : (int?)null,
                    EndYear = series.last_air_date.HasValue ? series.last_air_date.Value.Year : (int?)null,
                    CoverImageUrl = imageBasePath + series.poster_path,
                    Title = series.name.Substring(0, Math.Min(200, series.name.Length)),
                    LastAirDate = series.last_air_date
                };

                foreach (var writer in series.writers)
                {
                    var artist = mapper.Map<Artist>(writer);
                    newSeries.Writers.Add(new SeriesWriter
                    {
                        ArtistId = artist.ArtistId,
                        SeriesId = newSeries.SeriesId
                    });

                    if(!artistsToCreate.Any(a => a.ArtistId == artist.ArtistId))
                    {
                        artistsToCreate.Add(artist);
                    }
                }

                foreach (var cast in series.cast)
                {
                    var person = series.actors.First(a => a.id == cast.id);
                    var artist = mapper.Map<Artist>(person);

                    if (!artistsToCreate.Any(a => a.ArtistId == artist.ArtistId))
                    {
                        artistsToCreate.Add(artist);
                    }

                    var seriesActor = new SeriesActor
                    {
                        ArtistId = artist.ArtistId,
                        RoleName = cast.character,
                        Order = cast.order,
                        SeriesId = newSeries.SeriesId
                    };

                    newSeries.Cast.Add(seriesActor);
                }

                foreach (var genre in series.genres)
                {
                    var category = mapper.Map<Category>(genre);

                    if(!categoriesToCreate.Any(c => c.CategoryId == category.CategoryId))
                    {
                        categoriesToCreate.Add(category);
                    }

                    var seriesCategory = new SeriesCategory
                    {
                        CategoryId = category.CategoryId,
                        SeriesId = newSeries.SeriesId
                    };

                    if(!newSeries.Categories.Any(c => c.CategoryId == category.CategoryId))
                    {
                        newSeries.Categories.Add(seriesCategory);
                    }
                }

                foreach (var apiEpisode in series.episodes)
                {
                    var episode = new Episode
                    {
                        EpisodeId = apiEpisode.id,
                        Description = apiEpisode.overview.Substring(0, Math.Min(1500, apiEpisode.overview.Length)),
                        EpisodeTitle = apiEpisode.name.Substring(0, Math.Min(200, apiEpisode.name.Length)),
                        Release = apiEpisode.air_date,
                        EpisodeNumber = apiEpisode.episode_number,
                        Season = apiEpisode.season_number,
                        SeriesId = newSeries.SeriesId,
                        CoverImageUrl = imageBasePath + apiEpisode.still_path
                    };

                    newSeries.Episodes.Add(episode);
                }
                seriesToCreate.Add(newSeries);
            }

            using (var trans = context.Database.BeginTransaction(capBus, autoCommit: false))
            {
                try
                {
                    context.Categories.AddRange(categoriesToCreate);
                    context.Artists.AddRange(artistsToCreate);
                    context.Series.AddRange(seriesToCreate);
                    await context.SaveChangesAsync();
                    foreach (var series in seriesToCreate)
                    {
                        var seriesEvent = mapper.Map<SeriesCreatedEvent>(series);
                        capBus.Publish("browsingservice.series.created", seriesEvent);
                        foreach (var episode in series.Episodes)
                        {
                            var episodeEvent = mapper.Map<EpisodeCreatedEvent>(episode);
                            capBus.Publish("browsingservice.episode.created", episodeEvent);
                        }
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    trans.Rollback();
                }
            }
        }

    }
}
