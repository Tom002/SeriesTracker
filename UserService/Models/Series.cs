using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Models
{
    public class Series
    {
        [Required]
        public int SeriesId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        public ICollection<SeriesLiked> SeriesLiked { get; set; } = new List<SeriesLiked>();

        public ICollection<SeriesWatched> SeriesWatched { get; set; } = new List<SeriesWatched>();

        public ICollection<SeriesFollowed> SeriesFollowed { get; set; } = new List<SeriesFollowed>();
    }
}
