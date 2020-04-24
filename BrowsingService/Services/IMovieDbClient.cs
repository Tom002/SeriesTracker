using BrowsingService.Services.ApiResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Services
{
    public interface IMovieDbClient
    {
        public Task<SeriesApiResponse> GetSeries(int seriesId);
        public Task<SeasonApiResponse> GetSeasonEpisodes(int seriesId, int seasonNumber);
        public Task<PersonApiResponse> GetPerson(int personId);
        public Task<GenreApiResponse> GetGenres();
        public Task<TopSeriesResponse> GetTopSeries();
        public Task<CastApiResponse> GetCastForSeries(int seriesId);

    }
}
