using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Dto
{
    public class ArtistForListDto
    {
        public int ArtistId { get; set; }

        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? DeathDate { get; set; }

        public string City { get; set; }

        public string ImageUrl { get; set; }

        public int WriterOfCount { get; set; }

        public int AppearedInCount { get; set; }
    }
}
