using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Models
{
    public class Episode
    {
        [Required]
        public int EpisodeId { get; set; }

        public Series Series { get; set; }

        [Required]
        public int SeriesId { get; set; }

        [MaxLength(200)]
        public string EpisodeTitle { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        [Required]
        public int Season { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        public int LengthInMinutes { get; set; }

        public DateTime? Release { get; set; }

        public bool IsReleased { get; set; }

        public string CoverImageUrl { get; set; }
    }
}
