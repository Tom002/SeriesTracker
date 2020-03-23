using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Models
{
    public class SeriesWatched
    {
        [Required]
        public int SeriesId { get; set; }

        public Series Series { get; set; }

        [Required]
        public string ViewerId { get; set; }

        public Viewer Viewer { get; set; }
    }
}
