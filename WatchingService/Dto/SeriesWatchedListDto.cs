using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Dto
{
    public class SeriesWatchedListDto
    {
        public string ViewerId { get; set; }

        public List<int> SeriesWatchedIds { get; set; } = new List<int>();
    }
}
