using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Dto
{
    public class UserSeriesReviewListDto
    {
        public string SeriesTitle { get; set; }
        public int SeriesId { get; set; }
        public string ReviewTitle { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewText { get; set; }
        public int? Rating { get; set; }
    }
}
