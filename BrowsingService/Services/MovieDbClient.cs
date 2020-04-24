using BrowsingService.Services;
using BrowsingService.Services.ApiResponseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BrowsingService.Data
{
    public class MovieDbClient : IMovieDbClient
    {
        private readonly Uri baseUri = new Uri("https://api.themoviedb.org/3");

        private readonly string apiKey = "206fce684861b51cf8656b42d0357f5c";

        private async Task<T> GetAsync<T>(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }

        public async Task<SeriesApiResponse> GetSeries(int seriesId)
        {
            var builder = new UriBuilder($"{baseUri}/tv/{seriesId}");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["api_key"] = apiKey;
            builder.Query = query.ToString();

            return await GetAsync<SeriesApiResponse>(builder.Uri);
        }

        public async Task<SeasonApiResponse> GetSeasonEpisodes(int seriesId, int seasonNumber)
        {
            var builder = new UriBuilder($"{baseUri}/tv/{seriesId}/season/{seasonNumber}");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["api_key"] = apiKey;
            builder.Query = query.ToString();

            return await GetAsync<SeasonApiResponse>(builder.Uri);
        }

        public async Task<PersonApiResponse> GetPerson(int personId)
        {
            var builder = new UriBuilder($"{baseUri}/person/{personId}");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["api_key"] = apiKey;
            builder.Query = query.ToString();

            return await GetAsync<PersonApiResponse>(builder.Uri);
        }

        public async Task<GenreApiResponse> GetGenres()
        {
            var builder = new UriBuilder($"{baseUri}/genre/tv/list");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["api_key"] = apiKey;
            builder.Query = query.ToString();

            return await GetAsync<GenreApiResponse>(builder.Uri);
        }

        public async Task<TopSeriesResponse> GetTopSeries()
        {
            var builder = new UriBuilder($"{baseUri}/tv/top_rated");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["api_key"] = apiKey;
            builder.Query = query.ToString();

            return await GetAsync<TopSeriesResponse>(builder.Uri);
        }

        public async Task<CastApiResponse> GetCastForSeries(int seriesId)
        {
            var builder = new UriBuilder($"{baseUri}/tv/{seriesId}/credits");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["api_key"] = apiKey;
            builder.Query = query.ToString();

            return await GetAsync<CastApiResponse>(builder.Uri);
        }
    }
}
