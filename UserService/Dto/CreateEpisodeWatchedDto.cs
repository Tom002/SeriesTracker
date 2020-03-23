using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Dto
{
    public class CreateEpisodeWatchedDto
    {
        public DateTime WatchingDate { get; set; }

        public bool AddToDiary { get; set; }
    }
}
