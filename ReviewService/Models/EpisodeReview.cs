using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Models
{
    public class EpisodeReview
    {
        [Required]
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }

        [Required]
        public string ReviewerId { get; set; }
        public Reviewer Reviewer { get; set; }

        [Range(1, 5)]
        [Required]
        public int Rating { get; set; }
    }
}
