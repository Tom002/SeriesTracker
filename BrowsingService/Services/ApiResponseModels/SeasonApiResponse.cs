using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Services.ApiResponseModels
{
    public class SeasonApiResponse
    {
        public List<EpisodeResponse> episodes { get; set; }
    }

    public class EpisodeResponse
    {
        public DateTime? air_date { get; set; }
        public int episode_number { get; set; }
        public string name { get; set; }
        public string overview { get; set; }
        public int id { get; set; }
        public int season_number { get; set; }
        public string still_path { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }
}
