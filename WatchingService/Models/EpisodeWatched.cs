using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Models
{
    public class EpisodeWatched
    {
        [Required]
        public int EpisodeId { get; set; }

        public Episode Episode { get; set; }

        [Required]
        public string ViewerId { get; set; }

        public Viewer Viewer { get; set; }

        public DateTime WatchingDate { get; set; }

        public bool IsInDiary { get; set; }
    }
}
