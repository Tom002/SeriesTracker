using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BrowsingService.Data;
using BrowsingService.Dto;
using BrowsingService.Helpers;
using BrowsingService.Helpers.Pagination;
using BrowsingService.Interfaces;
using BrowsingService.Models;
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
        private readonly ISeriesService _seriesService;

        public SeriesController(BrowsingDbContext dbContext,
                                IMapper mapper,
                                ISeriesService seriesService)
        {
            _context = dbContext;
            _mapper = mapper;
            _seriesService = seriesService;
        }

        [HttpGet("categories")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories()
        {
            var categories = await _seriesService.GetCategories();
            return Ok(categories);
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PagedList<SeriesForListDto>>> SearchSeries([FromQuery] SeriesParams filter)
        {
            var seriesPaged = await _seriesService.SearchSeries(filter);
            Response.AddPagination(seriesPaged.CurrentPage,
                                   seriesPaged.PageSize,
                                   seriesPaged.TotalCount,
                                   seriesPaged.TotalPages);
            return Ok(seriesPaged);
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
                .Include(s => s.Reviews)
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
                    //Reviews = _mapper.Map<List<SeriesReviewDto>>(series.Reviews.ToList()),
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
        public async Task<ActionResult<List<EpisodeDto>>> GetSeasonEpisodes(int id, int seasonNumber)
        {
            var series = await _seriesService.GetSeries(id);
            if(series is Series)
            {
                var episodes = await _seriesService.GetSeasonEpisodes(series, seasonNumber);
                return Ok(episodes);
            }
            else
            {
                return NotFound();
            }
        }
    }
}