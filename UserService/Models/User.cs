using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Models
{
    public class User
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string City { get; set; }
        [MaxLength(400)]
        public string About { get; set; }
        public string ProfileImageUrl { get; set; }

        public ICollection<SeriesLiked> SeriesLiked { get; set; } = new List<SeriesLiked>();

        public ICollection<SeriesWatched> SeriesWatched { get; set; } = new List<SeriesWatched>();

        public ICollection<SeriesFollowed> SeriesFollowed { get; set; } = new List<SeriesFollowed>();

        public ICollection<EpisodeDiary> EpisodeDiary { get; set; } = new List<EpisodeDiary>();

        public ICollection<EpisodeCalendar> EpisodeCalendar { get; set; } = new List<EpisodeCalendar>();
    }
}
