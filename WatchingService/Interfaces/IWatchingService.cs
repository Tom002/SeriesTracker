using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchingService.Dto;
using WatchingService.Helpers;
using WatchingService.Helpers.Pagination;
using WatchingService.Models;

namespace WatchingService.Interfaces
{
    public interface IWatchingService
    {
        public Task<Viewer> GetViewer(string viewerId);

        public Task<Series> GetSeries(int seriesId);

        public Task<Episode> GetEpisode(int episodeId);

        public Task<PagedList<SeriesWatchedListDto>>
            GetSeriesWatchedList(Viewer viewer, PagingParams pagingParams);

        public Task<PagedList<SeriesLikedListDto>>
            GetSeriesLikedList(Viewer viewer, PagingParams pagingParams);

        public Task<ViewerSeriesWatchedInfoDto> GetSeriesWatchedInfo(Series series, string viewerId);

        public Task<bool> IsSeriesWatchedByViewer(int seriesId, string viewerId);

        public Task<bool> IsEpisodeWatchedByViewer(Episode episode, string viewerId);

        public Task<bool> IsSeriesLikedByViewer(int seriesId, string viewerId);

        public Task<bool> CreateSeriesWatched(Series series, string viewerId);

        public Task<bool> DeleteSeriesWatched(SeriesWatched seriesWatched);

        public Task<SeriesWatched> GetSeriesWatched(Series series, string viewerId);

        public Task<EpisodeWatched> GetEpisodeWatched(Episode episode, string viewerId);

        public Task<SeriesLiked> GetSeriesLiked(Series series, string viewerId);

        public Task<bool> CreateEpisodeWatched(Episode episode, string viewerId);

        public Task<bool> DeleteEpisodeWatched(EpisodeWatched episodeWatched);

        public Task<bool> CreateSeriesLiked(Series series, string viewerId);

        public Task<bool> DeleteSeriesLiked(SeriesLiked seriesLiked);
    }
}
