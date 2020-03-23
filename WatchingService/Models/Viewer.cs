using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Models
{
    public class Viewer
    {
        [Required]
        public string ViewerId { get; set; }

        public ICollection<SeriesWatched> SeriesWatched { get; set; } = new List<SeriesWatched>();

        public ICollection<EpisodeWatched> EpisodesWatched { get; set; } = new List<EpisodeWatched>();

        public ICollection<SeriesLiked> SeriesLiked { get; set; }
    }
}
