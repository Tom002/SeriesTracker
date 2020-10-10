using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Models
{
    public class Episode
    {
        [Required]
        public int EpisodeId { get; set; }

        [Required]
        public int SeriesId { get; set; }

        [MaxLength(200)]
        public string EpisodeTitle { get; set; }

        [Required]
        public int Season { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        public DateTime? Release { get; set; }

        public string CoverImageUrl { get; set; }

        public ICollection<EpisodeReview> Reviews { get; set; } = new List<EpisodeReview>();
    }
}
