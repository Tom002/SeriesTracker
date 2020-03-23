using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BrowsingService.Data;
using BrowsingService.Helpers;
using BrowsingService.Models;
using Common.Events;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrowsingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly BrowsingDbContext _context;

        private readonly IMapper _mapper;

        public SeriesController(BrowsingDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        [CapSubscribe("reviewService.seriesReview.created")]
        public async Task ReceiveSeriesReviewCreated(SeriesReviewCreatedEvent reviewEvent)
        {
            // Csak akkor hozom létre ha korábban még nem jött létre (idempotens feldolgozás)
            if (!await _context.SeriesReview.AnyAsync(review =>
                review.ReviewerId == reviewEvent.ReviewerId &&
                review.SeriesId == reviewEvent.SeriesId))
            {
                var review = _mapper.Map<SeriesReview>(reviewEvent);
                _context.SeriesReview.Add(review);
                await _context.SaveChangesAsync();
            }
        }

        [CapSubscribe("reviewService.episodeReview.created")]
        public async Task ReceiveEpisodeReviewCreated(EpisodeReviewCreatedEvent reviewEvent)
        {
            // Csak akkor hozom létre ha korábban még nem jött létre (idempotens feldolgozás)
            if (!await _context.EpisodeReview.AnyAsync(review =>
                review.ReviewerId == reviewEvent.ReviewerId &&
                review.EpisodeId == reviewEvent.EpisodeId))
            {
                var review = _mapper.Map<EpisodeReview>(reviewEvent);
                _context.EpisodeReview.Add(review);
                await _context.SaveChangesAsync();
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Series>>> GetSeries([FromQuery] SeriesFilter filter)
        {
            IQueryable<Series> seriesQuery = _context.Series;
            if (filter.TitleFilter != null)
            {
                seriesQuery = seriesQuery.Where(series => series.Title.Contains(filter.TitleFilter));
            }
            if (filter.CategoryId != 0)
            {
                seriesQuery = seriesQuery.Where(series => series.Categories.Any(category => category.CategoryId == filter.CategoryId));
            }
            var series = await seriesQuery.ToListAsync();
            return Ok(series);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Series>> GetSingleSeries(int id)
        {
            var series = await _context.Series
                .Include(s => s.Episodes)
                .Include(s => s.Writers)
                .Include(s => s.Cast)
                .Include(s => s.Categories)
                .Where(s => s.SeriesId == id)
                .FirstOrDefaultAsync();

            if (series == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(series);
            }
        }
    }
}