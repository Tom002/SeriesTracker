using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Dto
{
    public class SeriesLikedListDto
    {
        public string ViewerId { get; set; }

        public List<int> SeriesLikedIds { get; set; } = new List<int>();
    }
}
