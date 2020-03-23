using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Events;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewService.Data;
using ReviewService.Dto;
using ReviewService.Models;

namespace ReviewService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewDbContext _context;
        private readonly ICapPublisher _capBus;
        private readonly IMapper _mapper;

        public ReviewController(ReviewDbContext context, IMapper mapper, ICapPublisher capBus)
        {
            _context = context;
            _mapper = mapper;
            _capBus = capBus;
        }

        [CapSubscribe("identityservice.user.created")]
        public async Task ReceiveUserCreated(UserCreatedEvent userEvent)
        {
            if(!await _context.Reviewer.AnyAsync(reviewer => reviewer.ReviewerId == userEvent.UserId))
            {
                _context.Reviewer.Add(new Reviewer { ReviewerId = userEvent.UserId });
                await _context.SaveChangesAsync();
            }
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPost("/series")]
        public async Task<ActionResult> CreateSeriesReview(CreateSeriesReviewDto reviewDto)
        {
            var signedInUserId = User.FindFirst("sub").Value;
            if (reviewDto.ReviewerId != signedInUserId)
            {
                return Unauthorized();
            }

            var series = await _context.Series.FirstOrDefaultAsync(series => series.SeriesId == reviewDto.SeriesId);
            var user = await _context.Reviewer.FirstOrDefaultAsync(reviewer => reviewer.ReviewerId == reviewDto.ReviewerId);

            if(series == null || user == null)
            {
                return NotFound();
            }

            if(await _context.SeriesReview.AnyAsync(sr => 
                sr.ReviewerId == reviewDto.ReviewerId &&
                sr.SeriesId == reviewDto.SeriesId))
            {
                return BadRequest("You have already reviewed this series");
            }
            var seriesReview = _mapper.Map<SeriesReview>(reviewDto);


            using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: false))
            {
                try
                {
                    _context.SeriesReview.Add(seriesReview);
                    await _context.SaveChangesAsync();
                    var reviewEvent = _mapper.Map<SeriesReviewCreatedEvent>(seriesReview);
                    await _capBus.PublishAsync("reviewService.seriesReview.created", reviewEvent);
                    await trans.CommitAsync();
                    return NoContent();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                    return BadRequest("Error while saving series review");
                }
            }
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpPost("/episode")]
        public async Task<ActionResult> CreateEpisodeReview(CreateEpisodeReviewDto reviewDto)
        {
            var signedInUserId = User.FindFirst("sub").Value;
            if (reviewDto.ReviewerId != signedInUserId)
            {
                return Unauthorized();
            }

            var episode = await _context.Episode.FirstOrDefaultAsync(episode => episode.SeriesId == reviewDto.EpisodeId);
            var user = await _context.Reviewer.FirstOrDefaultAsync(reviewer => reviewer.ReviewerId == reviewDto.ReviewerId);

            if (episode == null || user == null)
            {
                return NotFound();
            }

            if (await _context.EpisodeReview.AnyAsync(er =>
                 er.ReviewerId == reviewDto.ReviewerId &&
                 er.EpisodeId == reviewDto.EpisodeId))
            {
                return BadRequest("You have already reviewed this episode");
            }

            var episodeReview = _mapper.Map<EpisodeReview>(reviewDto);

            using (var trans = _context.Database.BeginTransaction(_capBus, autoCommit: false))
            {
                try
                {
                    _context.EpisodeReview.Add(episodeReview);
                    await _context.SaveChangesAsync();
                    var reviewEvent = _mapper.Map<EpisodeReviewCreatedEvent>(episodeReview);
                    await _capBus.PublishAsync("reviewService.episodeReview.created", reviewEvent);
                    await trans.CommitAsync();
                    return NoContent();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                    return BadRequest("Error while saving episode review");
                }
            }
        }
    }
}