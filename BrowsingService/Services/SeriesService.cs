using AutoMapper;
using AutoMapper.QueryableExtensions;
using BrowsingService.Data;
using BrowsingService.Dto;
using BrowsingService.Helpers;
using BrowsingService.Helpers.Pagination;
using BrowsingService.Interfaces;
using BrowsingService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Services
{
    public class SeriesService : ISeriesService
    {
        private readonly BrowsingDbContext _context;
        private readonly IMapper _mapper;

        public SeriesService(BrowsingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Series> GetSeries(int seriesId)
        {
            return await _context.Series.FirstOrDefaultAsync(s => s.SeriesId == seriesId);
        }

        public async Task<List<CategoryDto>> GetCategories()
        {
            var categories = await _context.Categories
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return categories;
        }

        public async Task<PagedList<SeriesForListDto>> SearchSeries(SeriesParams filter)
        {
            var seriesQuery = _context.Series.AsQueryable();

            if (!String.IsNullOrEmpty(filter.TitleFilter))
            {
                seriesQuery = seriesQuery.Where(series => series.Title.Contains(filter.TitleFilter));
            }
            if (filter.CategoryId.HasValue)
            {
                seriesQuery = seriesQuery.Where(series => series.Categories
                    .Any(category => category.CategoryId == filter.CategoryId.Value));
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

            var seriesMappedList = seriesQuery.ProjectTo<SeriesForListDto>(_mapper.ConfigurationProvider);
            var seriesPaged = await PagedList<SeriesForListDto>.CreateAsync(seriesMappedList, filter.PageNumber, filter.PageSize);
            return seriesPaged;
        }

        public async Task<List<EpisodeDto>> GetSeasonEpisodes(Series series, int season)
        {
            var episodeList = await _context.Episodes
                .Where(e => e.SeriesId == series.SeriesId &&
                            e.Season == season)
                .ProjectTo<EpisodeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return episodeList;
        }
    }
}
