using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Dto
{
    public class SeriesWatchedListDto
    {
        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        public int StartYear { get; set; }

        public int? EndYear { get; set; }

        public int SeriesId { get; set; }
    }
}
