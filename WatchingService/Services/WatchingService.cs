using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Events;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchingService.Data;
using WatchingService.Dto;
using WatchingService.Helpers;
using WatchingService.Helpers.Pagination;
using WatchingService.Interfaces;
using WatchingService.Models;

namespace WatchingService.Services
{
    public class WatchingService : IWatchingService
    {
        private readonly WatchingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICapPublisher _capPublisher;
        private readonly ILogger _logger;

        public WatchingService(WatchingDbContext context,
                               IMapper mapper,
                               ICapPublisher capPublisher,
                               ILogger<WatchingService> logger)
        {
            _context = context;
            _mapper = mapper;
            _capPublisher = capPublisher;
            _logger = logger;
        }

        public async Task<Viewer> GetViewer(string viewerId)
        {
            return await _context.Viewer.FirstOrDefaultAsync(v => v.ViewerId == viewerId);
        }

        public async Task<Series> GetSeries(int seriesId)
        {
            return await _context.Series.FirstOrDefaultAsync(s => s.SeriesId == seriesId);
        }

        public async Task<Episode> GetEpisode(int episodeId)
        {
            return await _context.Episode.FirstOrDefaultAsync(e => e.EpisodeId == episodeId);
        }

        public async Task<PagedList<SeriesWatchedListDto>> 
            GetSeriesWatchedList(Viewer viewer, PagingParams pagingParams)
        {
            var seriesWatchedQuery = _context.SeriesWatched
                .Where(sw => sw.ViewerId == viewer.ViewerId)
                .ProjectTo<SeriesWatchedListDto>(_mapper.ConfigurationProvider);

            var pagedList = await PagedList<SeriesWatchedListDto>
                .CreateAsync(seriesWatchedQuery, pagingParams.PageNumber, pagingParams.PageSize);

            return pagedList;
        }

        public async Task<PagedList<SeriesLikedListDto>>
            GetSeriesLikedList(Viewer viewer, PagingParams pagingParams)
        {
            var seriesLikedQuery = _context.SeriesLiked
                .Where(sw => sw.ViewerId == viewer.ViewerId)
                .ProjectTo<SeriesLikedListDto>(_mapper.ConfigurationProvider);

            var pagedList = await PagedList<SeriesLikedListDto>
                .CreateAsync(seriesLikedQuery, pagingParams.PageNumber, pagingParams.PageSize);

            return pagedList;
        }

        public async Task<ViewerSeriesWatchedInfoDto> GetSeriesWatchedInfo(Series series, string viewerId)
        {
            var seriesWatched = await _context.SeriesWatched
                .FirstOrDefaultAsync(sw => sw.SeriesId == series.SeriesId &&
                                           sw.ViewerId == viewerId);

            var seriesLiked = await _context.SeriesLiked
                .FirstOrDefaultAsync(sl => sl.SeriesId == series.SeriesId &&
                                           sl.ViewerId == viewerId);

            var response = new ViewerSeriesWatchedInfoDto();

            if (seriesWatched is SeriesWatched)
            {
                var episodesWatchedIdList = await _context.EpisodeWatched
                    .Where(ew => ew.ViewerId == viewerId &&
                                 ew.Episode.SeriesId == series.SeriesId)
                    .Select(ew => ew.EpisodeId)
                    .ToListAsync();

                response.IsWatchingSeries = true;
                response.EpisodesWatchedIdList = episodesWatchedIdList;;
            }
            else
            {
                response.IsWatchingSeries = false;
            }
            response.HasLikedSeries = seriesLiked is SeriesLiked;

            return response;
        }

        public async Task<bool> IsSeriesWatchedByViewer(int seriesId, string viewerId)
        {
            return await _context.SeriesWatched
                .AnyAsync(sw => sw.SeriesId == seriesId &&
                                sw.ViewerId == viewerId);
        }

        public async Task<bool> IsSeriesLikedByViewer(int seriesId, string viewerId)
        {
            return await _context.SeriesLiked
                .AnyAsync(sw => sw.SeriesId == seriesId &&
                                sw.ViewerId == viewerId);
        }

        public async Task<bool> IsEpisodeWatchedByViewer(Episode episode, string viewerId)
        {
            return await _context.EpisodeWatched
                .AnyAsync(ew => ew.EpisodeId == episode.EpisodeId &&
                                ew.ViewerId == viewerId);
        }

        public async Task<SeriesWatched> GetSeriesWatched(Series series, string viewerId)
        {
            return await _context.SeriesWatched
                .FirstOrDefaultAsync(sw => sw.SeriesId == series.SeriesId &&
                                           sw.ViewerId == viewerId);
        }

        public async Task<EpisodeWatched> GetEpisodeWatched(Episode episode, string viewerId)
        {
            return await _context.EpisodeWatched
                .FirstOrDefaultAsync(ew => ew.EpisodeId == episode.EpisodeId &&
                                           ew.ViewerId == viewerId);
        }

        public async Task<SeriesLiked> GetSeriesLiked(Series series, string viewerId)
        {
            return await _context.SeriesLiked
                .FirstOrDefaultAsync(sl => sl.SeriesId == series.SeriesId &&
                                           sl.ViewerId == viewerId);
        }

        public async Task<bool> CreateSeriesWatched(Series series, string viewerId)
        {
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
            {
                try
                {
                    var seriesWatched = new SeriesWatched
                    {
                        SeriesId = series.SeriesId,
                        ViewerId = viewerId
                    };
                    _context.SeriesWatched.Add(seriesWatched);
                    await _context.SaveChangesAsync();
                    var seriesWatchedEvent = _mapper.Map<SeriesWatchedEvent>(seriesWatched);
                    await _capPublisher.SendEvent(EventInfo.SeriesWatchedCreated, seriesWatchedEvent);
                    await trans.CommitAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(e, $"Unexpected error while creating series watched" +
                            $" SeriesId:{series.SeriesId} UserId:{viewerId}");
                    return false;
                }
            }
        }

        public async Task<bool> DeleteSeriesWatched(SeriesWatched seriesWatched)
        {
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
            {
                try
                {
                    _context.SeriesWatched.Remove(seriesWatched);
                    await _context.SaveChangesAsync();
                    var seriesWatchedEvent = _mapper.Map<SeriesWatchedDeletedEvent>(seriesWatched);
                    await _capPublisher.SendEvent(EventInfo.SeriesWatchedDeleted, seriesWatchedEvent);
                    await trans.CommitAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(e, $"Unexpected error while deleting series watched" +
                            $" SeriesId:{seriesWatched.SeriesId} UserId:{seriesWatched.ViewerId}");
                    return false;
                }
            }
        }

        public async Task<bool> CreateEpisodeWatched(Episode episode, string viewerId)
        {
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
            {
                try
                {
                    if (!await IsSeriesWatchedByViewer(episode.SeriesId, viewerId))
                    {
                        var seriesWatched = new SeriesWatched 
                        { 
                            SeriesId = episode.SeriesId,
                            ViewerId = viewerId
                        };
                        _context.SeriesWatched.Add(seriesWatched);
                        await _context.SaveChangesAsync();
                        var seriesWatchedEvent = _mapper.Map<SeriesWatchedEvent>(seriesWatched);
                        await _capPublisher.SendEvent(EventInfo.SeriesWatchedCreated, seriesWatchedEvent);
                    }

                    var episodeWatched = new EpisodeWatched
                    {
                        EpisodeId = episode.EpisodeId,
                        ViewerId = viewerId
                    };
                    _context.EpisodeWatched.Add(episodeWatched);
                    var episodeWatchedEvent = _mapper.Map<EpisodeWatchedEvent>(episodeWatched);
                    await _context.SaveChangesAsync();
                    await _capPublisher.SendEvent(EventInfo.EpisodeWatchedCreated, episodeWatchedEvent);
                    await trans.CommitAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(e, $"Unexpected error while creating episode watched" +
                            $" EpisodeId:{episode.EpisodeId} UserId:{viewerId}");
                    return false;
                }
            }
        }

        public async Task<bool> DeleteEpisodeWatched(EpisodeWatched episodeWatched)
        {
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
            {
                try
                {
                    _context.EpisodeWatched.Remove(episodeWatched);
                    await _context.SaveChangesAsync();
                    var episodeWatchedDeletedEvent = _mapper.Map<EpisodeWatchedDeletedEvent>(episodeWatched);
                    await _capPublisher.SendEvent(EventInfo.EpisodeWatchedDeleted,
                                                  episodeWatchedDeletedEvent);
                    await trans.CommitAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(e, $"Unexpected error while deleting episode watched" +
                            $" EpisodeId:{episodeWatched.EpisodeId} UserId:{episodeWatched.ViewerId}");
                    return false;
                }
            }
        }

        public async Task<bool> CreateSeriesLiked(Series series, string viewerId)
        {
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
            {
                try
                {
                    var seriesLiked = new SeriesLiked
                    {
                        SeriesId = series.SeriesId,
                        ViewerId = viewerId
                    };
                    _context.SeriesLiked.Add(seriesLiked);
                    await _context.SaveChangesAsync();
                    var seriesLikedEvent = _mapper.Map<SeriesLikedEvent>(seriesLiked);
                    await _capPublisher.SendEvent(EventInfo.SeriesLikedCreated, seriesLikedEvent);
                    await trans.CommitAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(e, $"Unexpected error while creating series liked" +
                            $" SeriesId:{series.SeriesId} UserId:{viewerId}");
                    return false;
                }
            }
        }

        public async Task<bool> DeleteSeriesLiked(SeriesLiked seriesLiked)
        {
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
            {
                try
                {
                    _context.SeriesLiked.Remove(seriesLiked);
                    await _context.SaveChangesAsync();
                    var seriesLikedDeletedEvent = _mapper.Map<SeriesLikedDeletedEvent>(seriesLiked);
                    await _capPublisher.SendEvent(EventInfo.SeriesLikedDeleted,
                                                  seriesLikedDeletedEvent);
                    await trans.CommitAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(e, $"Unexpected error while deleting series liked" +
                            $" SeriesId:{seriesLiked.SeriesId} UserId:{seriesLiked.ViewerId}");
                    return false;
                }
            }
        }
    }
}
