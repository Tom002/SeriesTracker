using Common;
using ReviewService.Dto;
using ReviewService.Helpers.Pagination;
using ReviewService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Interfaces
{
    public interface IReviewService
    {
        public Task<Reviewer> GetReviewer(string userId);

        public Task<Series> GetSeries(int seriesId);

        public Task<SeriesReview> GetSeriesReview(int seriesId, string userId);

        public Task<Episode> GetEpisode(int episodeId);

        public Task<EpisodeReview> GetEpisodeReview(int episodeId, string userId);

        public Task<bool> CreateEpisodeReview(int episodeId, string userId, EpisodeReviewManipulationDto episodeReview);

        public Task<bool> UpdateEpisodeReview(EpisodeReview episodeReview, EpisodeReviewManipulationDto updateDto);

        public Task<PagedList<SeriesReviewListDto>> GetSeriesReviews(Series series, PagingParams pagingParams);
    }
}
