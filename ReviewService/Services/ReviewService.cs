using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Events;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReviewService.Data;
using ReviewService.Dto;
using ReviewService.Helpers;
using ReviewService.Helpers.Pagination;
using ReviewService.Interfaces;
using ReviewService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ReviewDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICapPublisher _capPublisher;
        private readonly ILogger _logger;

        public ReviewService(ReviewDbContext context,
                             IMapper mapper,
                             ICapPublisher capPublisher,
                             ILogger<ReviewService> logger)
        {
            _context = context;
            _mapper = mapper;
            _capPublisher = capPublisher;
            _logger = logger;
        }

        public async Task<Reviewer> GetReviewer(string userId)
        {
            return await _context.Reviewer.FirstOrDefaultAsync(r => r.ReviewerId == userId);
        }

        public async Task<Series> GetSeries(int seriesId)
        {
            return await _context.Series.FirstOrDefaultAsync(s => s.SeriesId == seriesId);
        }

        public async Task<SeriesReview> GetSeriesReview(int seriesId, string userId)
        {
            return await _context.SeriesReview.FirstOrDefaultAsync(r => r.ReviewerId == userId &&
                                                                        r.SeriesId == seriesId);
        }

        public async Task<Episode> GetEpisode(int episodeId)
        {
            return await _context.Episode.FirstOrDefaultAsync(episode => episode.EpisodeId == episodeId);
        }

        public async Task<EpisodeReview> GetEpisodeReview(int episodeId, string userId)
        {
            return await _context.EpisodeReview
                .FirstOrDefaultAsync(er => er.ReviewerId == userId &&
                                           er.EpisodeId == episodeId);
        }

        public async Task<bool> CreateEpisodeReview(int episodeId,
                                                    string userId,
                                                    EpisodeReviewManipulationDto episodeReview)
        {
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
            {
                try
                {
                    var createdReview = _mapper.Map<EpisodeReview>(episodeReview);
                    _context.EpisodeReview.Add(createdReview);
                    await _context.SaveChangesAsync();
                    var reviewEvent = _mapper.Map<EpisodeReviewCreatedEvent>(createdReview);
                    await _capPublisher.SendEvent(EventInfo.EpisodeReviewCreated, reviewEvent);
                    await trans.CommitAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(e, $"Unexpected error while creating episode review" +
                            $" EpisodeId: {episodeId} UserId:{userId}");
                    return false;
                }
            }
        }

        public async Task<bool> UpdateEpisodeReview(EpisodeReview episodeReview, EpisodeReviewManipulationDto updateDto)
        {
            using (var trans = _context.Database.BeginTransaction(_capPublisher, autoCommit: false))
            {
                try
                {
                    _mapper.Map(updateDto, episodeReview);
                    await _context.SaveChangesAsync();
                    var reviewEvent = _mapper.Map<EpisodeReviewUpdatedEvent>(episodeReview);
                    await _capPublisher.SendEvent(EventInfo.EpisodeReviewUpdated, reviewEvent);
                    await trans.CommitAsync();
                    return true;
                }
                catch (Exception e)
                {
                    await trans.RollbackAsync();
                    _logger.LogError(e, $"Unexpected error while updating episode review" +
                            $" EpisodeId: {episodeReview.EpisodeId} UserId:{episodeReview.ReviewerId}");
                    return false;
                }
            }
            
        }

        public async Task<PagedList<SeriesReviewListDto>> GetSeriesReviews(Series series, PagingParams pagingParams)
        {
            var reviewsQuery = _context.SeriesReview
                .Where(s => s.SeriesId == series.SeriesId &&
                            !String.IsNullOrEmpty(s.ReviewTitle) &&
                            !String.IsNullOrEmpty(s.ReviewText))
                .ProjectTo<SeriesReviewListDto>(_mapper.ConfigurationProvider);

            var pagedResults = await PagedList<SeriesReviewListDto>
                .CreateAsync(reviewsQuery, pagingParams.PageNumber, pagingParams.PageSize);
            return pagedResults;
        }
    }
}
