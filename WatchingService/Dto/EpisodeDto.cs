using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Dto
{
    public class EpisodeDto
    {
        public int EpisodeId { get; set; }

        public int SeriesId { get; set; }

        public int Season { get; set; }

        public int EpisodeNumber { get; set; }

        public string CoverImageUrl { get; set; }
    }
}
