using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Dto
{
    public class ActorInSeriesDto
    {
        public int SeriesId { get; set; }
        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
        public string RoleName { get; set; }
    }
}
