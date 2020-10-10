using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Events;
using Common.Interfaces;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReviewService.Data;
using ReviewService.Dto;
using ReviewService.Helpers;
using ReviewService.Helpers.Pagination;
using ReviewService.Helpers.RequestContext;
using ReviewService.Interfaces;
using ReviewService.Models;

namespace ReviewService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewDbContext _context;
        private readonly IRequestContext _requestContext;
        private readonly ICapPublisher _capBus;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IReviewService _reviewService;

        public ReviewController(ReviewDbContext context,
                                IMapper mapper,
                                ICapPublisher capBus,
                                IRequestContext requestContext,
                                ILogger<ReviewController> logger,
                                IReviewService reviewService)
        {
            _context = context;
            _requestContext = requestContext;
            _mapper = mapper;
            _capBus = capBus;
            _logger = logger;
            _reviewService = reviewService;
        }

        [Authorize]
        [HttpPut("series/{seriesId}/rate")]
        public async Task<ActionResult<UserSeriesReviewInfoDto>> RateSeries(int seriesId, [FromBody] AddSeriesReviewRatingDto ratingDto)
        {
            var userId = _requestContext.UserId;
            var reviewer = await _reviewService.GetReviewer(userId);
            var series = await _reviewService.GetSeries(seriesId);
            if (reviewer is Reviewer && series is Series)
            {
                var review = await _reviewService.GetSeriesReview(seriesId, userId);
                if (review is SeriesReview)
                {
                    // Review már létezik, a meglévőt szerkesztjük

                    using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: false))
                    {
                        try
                        {
                            _mapper.Map(ratingDto, review);
                            await _context.SaveChangesAsync();
                            var reviewEvent = _mapper.Map<SeriesReviewUpdatedEvent>(review);
                            await _capBus.SendEvent("reviewService.seriesReview.updated", reviewEvent);
                            await trans.CommitAsync();
                            
                            var reviewInfo = _mapper.Map<UserSeriesReviewInfoDto>(review);
                            return Ok(reviewInfo);
                        }
                        catch (Exception e)
                        {
                            await trans.RollbackAsync();
                            _logger.LogError(e, $"Unexpected error while updating series review" +
                            $" UserId: {reviewer.ReviewerId} SeriesId:{series.SeriesId}");
                            return BadRequest("Error while saving series review");
                        }
                    }
                }
                else
                {
                    // Review még nem létezik, újat hozunk létre
                    using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: false))
                    {
                        try
                        {
                            var createdReview = _mapper.Map<SeriesReview>(ratingDto);
                            createdReview.ReviewerId = userId;
                            createdReview.SeriesId = series.SeriesId;
                            _context.SeriesReview.Add(createdReview);
                            await _context.SaveChangesAsync();
                            var reviewEvent = _mapper.Map<SeriesReviewCreatedEvent>(createdReview);
                            await _capBus.SendEvent("reviewService.seriesReview.created", reviewEvent);
                            await trans.CommitAsync();

                            var reviewInfo = _mapper.Map<UserSeriesReviewInfoDto>(createdReview);
                            return Ok(reviewInfo);
                        }
                        catch (Exception e)
                        {
                            await trans.RollbackAsync();
                            _logger.LogError(e, $"Unexpected error while creating series review" +
                            $" UserId: {reviewer.ReviewerId} SeriesId:{series.SeriesId}");
                            return BadRequest("Error while saving series review");
                        }
                    }
                }
            }
            return NotFound();
        }

        [Authorize]
        [HttpPut("series/{seriesId}/review")]
        public async Task<ActionResult<UserSeriesReviewInfoDto>> ReviewSeries(int seriesId, [FromBody] AddSeriesReviewTextDto reviewDto)
        {
            var userId = _requestContext.UserId;
            var reviewer = await _reviewService.GetReviewer(userId);
            var series = await _reviewService.GetSeries(seriesId);
            if (reviewer is Reviewer && series is Series)
            {
                var review = await _reviewService.GetSeriesReview(seriesId, userId);
                if (review is SeriesReview)
                {
                    using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: false))
                    {
                        try
                        {
                            _mapper.Map(reviewDto, review);
                            await _context.SaveChangesAsync();
                            var reviewEvent = _mapper.Map<SeriesReviewUpdatedEvent>(review);
                            await _capBus.SendEvent("reviewService.seriesReview.updated", reviewEvent);
                            await trans.CommitAsync();

                            var reviewInfo = _mapper.Map<UserSeriesReviewInfoDto>(review);
                            return Ok(reviewInfo);
                        }
                        catch (Exception e)
                        {
                            await trans.RollbackAsync();
                            _logger.LogError(e, $"Unexpected error while updating series review" +
                            $" UserId: {reviewer.ReviewerId} SeriesId:{series.SeriesId}");
                            return BadRequest("Error while saving series review");
                        }
                    }
                }
                else
                {
                    using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: false))
                    {
                        try
                        {
                            // Review még nem létezik, újat hozunk létre
                            var createdReview = _mapper.Map<SeriesReview>(reviewDto);
                            createdReview.ReviewerId = userId;
                            createdReview.SeriesId = series.SeriesId;
                            _context.SeriesReview.Add(createdReview);
                            await _context.SaveChangesAsync();
                            var reviewEvent = _mapper.Map<SeriesReviewUpdatedEvent>(review);
                            await _capBus.SendEvent("reviewService.seriesReview.created", reviewEvent);
                            await trans.CommitAsync();

                            var reviewInfo = _mapper.Map<UserSeriesReviewInfoDto>(createdReview);
                            return Ok(reviewInfo);
                        }
                        catch (Exception e)
                        {
                            await trans.RollbackAsync();
                            _logger.LogError(e, $"Unexpected error while updating series review" +
                            $" UserId: {reviewer.ReviewerId} SeriesId:{series.SeriesId}");
                            return BadRequest("Error while saving series review");
                        }
                    }
                    
                }
            }
            return NotFound();
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpGet("series/{seriesId}/myreview")]
        public async Task<ActionResult<UserSeriesReviewInfoDto>> GetUserReviewForSeries(int seriesId)
        {
            var userId = _requestContext.UserId;
            var series = await _reviewService.GetSeries(seriesId);
            if(series is Series)
            {
                var review = await _reviewService.GetSeriesReview(seriesId, userId);
                if (review is SeriesReview)
                {
                    var response = _mapper.Map<UserSeriesReviewInfoDto>(review);
                    return Ok(response);
                }
                else
                {
                    return Ok(new UserSeriesReviewInfoDto { IsReviewedByUser = false });
                }
            }
            else
            {
                return NotFound();
            }
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<PagedList<UserSeriesReviewListDto>>> GetUserSeriesReviews(
            string userId, 
            [FromQuery] ReviewParams reviewParams)
        {
            var user = await _context.Reviewer.FirstOrDefaultAsync(r => r.ReviewerId == userId);
            if(user is Reviewer)
            {
                var userReviewsQuery = _context.SeriesReview
                    .Where(r => r.ReviewerId == user.ReviewerId)
                    .ProjectTo<UserSeriesReviewListDto>(_mapper.ConfigurationProvider);

                var pagedReviews = await PagedList<UserSeriesReviewListDto>
                    .CreateAsync(userReviewsQuery, reviewParams.PageNumber, reviewParams.PageSize);

                return Ok(pagedReviews);
            }
            return NotFound();
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPut("episode/{episodeId}")]
        public async Task<ActionResult> ReviewEpisode(int episodeId, EpisodeReviewManipulationDto reviewDto)
        {
            var userId = _requestContext.UserId;
            var episode = await _reviewService.GetEpisode(episodeId);
            if (episode is Episode)
            {
                var episodeReview = await _reviewService.GetEpisodeReview(episodeId, userId);
                if (episodeReview is EpisodeReview)
                {
                    var reviewUpdateSuccess = await _reviewService.UpdateEpisodeReview(episodeReview, reviewDto);
                    if (reviewUpdateSuccess)
                        return NoContent();
                    return BadRequest("Unexpected error while saving episode review");
                }
                else
                {
                    var reviewCreatedSuccess = await _reviewService.CreateEpisodeReview(episodeId, userId, reviewDto);
                    if (reviewCreatedSuccess)
                        return NoContent();
                    return BadRequest("Unexpected error while saving episode review");
                }
            }
            else
            {
                return NotFound();
            }
            
        }


        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpGet("series/{seriesId}")]
        public async Task<ActionResult<PagedList<SeriesReviewListDto>>> 
            ListSeriesReviews(int seriesId, [FromQuery] PagingParams pagingParams)
        {
            var series = await _reviewService.GetSeries(seriesId);
            if(series is Series)
            {
                var reviews = await _reviewService.GetSeriesReviews(series, pagingParams);
                return Ok(reviews);
            }  
            return NotFound();
        }
    }
}