using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Services.ApiResponseModels
{
    public class PersonApiResponse
    {
        public DateTime? birthday { get; set; }
        public DateTime? deathday { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int gender { get; set; }
        public string biography { get; set; }
        public string place_of_birth { get; set; }
        public string profile_path { get; set; }
        public bool adult { get; set; }
    }
}
