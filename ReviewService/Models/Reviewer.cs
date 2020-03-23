using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Models
{
    public class Reviewer
    {
        public string ReviewerId { get; set; }
        public string UserName { get; set; }

        public ICollection<EpisodeReview> EpisodeReviews { get; set; } = new List<EpisodeReview>();

        public ICollection<SeriesReview> SeriesReviews { get; set; } = new List<SeriesReview>();
    }
}
