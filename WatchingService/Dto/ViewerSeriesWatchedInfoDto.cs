using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Dto
{
    public class ViewerSeriesWatchedInfoDto
    {
        public bool IsWatchingSeries { get; set; }

        public bool HasLikedSeries { get; set; }

        public List<int> EpisodesWatchedIdList { get; set; } = new List<int>();
    }
}
