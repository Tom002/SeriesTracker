using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Models
{
    public class Episode
    {
        [Required]
        public int EpisodeId { get; set; }

        [Required]
        public int SeriesId { get; set; }

        [MaxLength(50)]
        public string EpisodeTitle { get; set; }

        [Required]
        public int Season { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        public DateTime Release { get; set; }

        public ICollection<EpisodeDiary> UserDiaries { get; set; } = new List<EpisodeDiary>();
    }
}
