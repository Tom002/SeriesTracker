using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Models
{
    public class EpisodeReview
    {
        [Required]
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }

        [Required]
        public string ReviewerId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
