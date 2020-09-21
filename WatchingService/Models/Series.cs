using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Models
{
    public class Series
    {
        [Required]
        public int SeriesId { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        public ICollection<SeriesWatched> SeriesWatched { get; set; }

        public ICollection<SeriesLiked> SeriesLiked { get; set; }
    }
}
