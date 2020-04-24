using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BrowsingService.Data;
using BrowsingService.Dto;
using BrowsingService.Helpers;
using BrowsingService.Helpers.Pagination;
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

        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            var categoriesToReturn = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(categoriesToReturn);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Series>>> GetSeries([FromQuery] SeriesParams filter)
        {
            Console.WriteLine(Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD"));
            IQueryable<Series> seriesQuery = _context.Series
                .Include(series => series.Categories).ThenInclude(sc => sc.Category)
                .Include(series => series.Reviews);

            if (!String.IsNullOrEmpty(filter.TitleFilter))
            {
                seriesQuery = seriesQuery.Where(series => series.Title.Contains(filter.TitleFilter));
            }
            if (filter.CategoryId != 0)
            {
                seriesQuery = seriesQuery.Where(series => series.Categories.Any(category => category.CategoryId == filter.CategoryId));
            }
            switch (filter.Sort)
            {
                case SeriesParams.SortBy.HighestRating:
                    seriesQuery = seriesQuery.OrderByDescending(series => series.Reviews.Average(r => r.Rating));
                    break;
                case SeriesParams.SortBy.LowestRating:
                    seriesQuery = seriesQuery.OrderBy(series => series.Reviews.Average(r => r.Rating));
                    break;
                case SeriesParams.SortBy.MostReviewed:
                    seriesQuery = seriesQuery.OrderByDescending(series => series.Reviews.Count());
                    break;
                case SeriesParams.SortBy.Alphabetical:
                    seriesQuery = seriesQuery.OrderBy(series => series.Title);
                    break;
                case SeriesParams.SortBy.AlphabeticalDescending:
                    seriesQuery = seriesQuery.OrderByDescending(series => series.Title);
                    break;
                default:
                    break;
            }

            var seriesToList = seriesQuery.Select(series => new SeriesForListDto
            {
                SeriesId = series.SeriesId,
                Categories = _mapper.Map<List<CategoryDto>>(series.Categories.Select(sc => sc.Category).ToList()),
                CoverImageUrl = series.CoverImageUrl,
                Title = series.Title,
                StartYear = series.StartYear,
                EndYear = series.EndYear,
                AverageRating = series.Reviews.Average(r => r.Rating)
            });

            var series = await PagedList<SeriesForListDto>.CreateAsync(seriesToList, filter.PageNumber, filter.PageSize);
            Response.AddPagination(series.CurrentPage, series.PageSize, series.TotalCount, series.TotalPages);
            return Ok(series);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Series>> GetSingleSeries(int id)
        {
            var series = await _context.Series
                .Include(s => s.Episodes)
                .Include(s => s.Writers).ThenInclude(sw => sw.Artist)
                .Include(s => s.Cast).ThenInclude(sc => sc.Artist)
                .Include(s => s.Categories).ThenInclude(sc => sc.Category)
                .Where(s => s.SeriesId == id)
                .FirstOrDefaultAsync();

            if (series == null)
            {
                return NotFound();
            }
            else
            {
                var seriesDetails = new SeriesDetailsDto
                {
                    SeriesId = series.SeriesId,
                    Categories = _mapper.Map<List<CategoryDto>>(series.Categories.Select(sc => sc.Category).ToList()),
                    Cast = _mapper.Map<List<ActorWithRoleDto>>(series.Cast.ToList()),
                    Writers = series.Writers.Select(s => new WriterDto { ArtistId = s.ArtistId, Name = s.Artist.Name}).ToList(),
                    RatingAverage = series.Reviews.Any() ? series.Reviews.Average(r => r.Rating) : 0,
                    RatingsCount = series.Reviews.Count(),
                    CoverImageUrl = series.CoverImageUrl,
                    Description = series.Description,
                    SeasonNumbers = series.Episodes.Select(e => e.Season).Distinct().ToList(),
                    StartYear = series.StartYear,
                    EndYear = series.EndYear,
                    Title = series.Title
                };
                return Ok(seriesDetails);
            }
        }

        [HttpGet("{id}/season/{seasonNumber}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Series>> GetSeasonEpisodes(int id, int seasonNumber)
        {
            var series = await _context.Series
                .Include(s => s.Episodes)
                .Where(s => s.SeriesId == id)
                .FirstOrDefaultAsync();

            series.Episodes = series.Episodes.Where(e => e.Season == seasonNumber).ToList();

            if(series != null && series.Episodes.Any())
            {
                var episodeList = _mapper.Map<List<EpisodeDto>>(series.Episodes);
                return Ok(episodeList);
            }
            else
            {
                return NotFound("Series or season not found");
            }
        }
    }
}