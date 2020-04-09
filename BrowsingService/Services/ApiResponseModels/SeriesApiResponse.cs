using System;
using System.Collections.Generic;

namespace BrowsingService.Services.ApiResponseModels
{
    public class SeriesApiResponse
    {
        public string backdrop_path { get; set; }
        public IList<CreatedBy> created_by { get; set; }
        public IList<int> episode_run_time { get; set; }
        public DateTime? first_air_date { get; set; }
        public List<Genre> genres { get; set; } = new List<Genre>();
        public int id { get; set; }
        public bool in_production { get; set; }
        public DateTime? last_air_date { get; set; }
        public string name { get; set; }
        public int number_of_episodes { get; set; }
        public int number_of_seasons { get; set; }
        public string original_name { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public IList<Season> seasons { get; set; }
    }

    public class CreatedBy
    {
        public int id { get; set; }
        public string name { get; set; }
        public int gender { get; set; }
        public string profile_path { get; set; }
    }

    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
    }


    public class Season
    {
        public DateTime? air_date { get; set; }
        public int episode_count { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public string poster_path { get; set; }
        public int season_number { get; set; }
    }

    public class TopSeriesResponse
    {
        public List<SeriesMinimal> results { get; set; }
    }

    public class SeriesMinimal
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
