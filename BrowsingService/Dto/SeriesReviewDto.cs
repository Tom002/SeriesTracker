using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Dto
{
    public class SeriesReviewDto
    {
        public int SeriesId { get; set; }
        public string ReviewerId { get; set; }
        public string ReviewTitle { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
    }
}
