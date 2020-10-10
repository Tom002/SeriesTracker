using BrowsingService.Dto;
using BrowsingService.Helpers;
using BrowsingService.Helpers.Pagination;
using BrowsingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Interfaces
{
    public interface ISeriesService
    {
        public Task<Series> GetSeries(int seriesId);
        public Task<List<CategoryDto>> GetCategories();
        public Task<PagedList<SeriesForListDto>> SearchSeries(SeriesParams filter);
        public Task<List<EpisodeDto>> GetSeasonEpisodes(Series series, int season);
    }
}
