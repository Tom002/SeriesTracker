using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Dto
{
    public class SeriesEpisodesWatchedListDto
    {
        public string ViewerId { get; set; }

        public int SeriesId { get; set; }

        public List<int> EpisodesWatchedIds { get; set; } = new List<int>();
    }
}
