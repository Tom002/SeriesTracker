using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Services.ApiResponseModels
{
    public class CompleteSeries
    {
        public string backdrop_path { get; set; }
        public List<PersonApiResponse> writers { get; set; } = new List<PersonApiResponse>();
        public List<PersonApiResponse> actors { get; set; } = new List<PersonApiResponse>();
        public List<Cast> cast { get; set; } = new List<Cast>();
        public List<EpisodeResponse> episodes { get; set; } = new List<EpisodeResponse>();
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
    }
}
