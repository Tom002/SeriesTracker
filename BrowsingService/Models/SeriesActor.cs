using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Models
{
    public class SeriesActor
    {
        [Required]
        public int SeriesId { get; set; }

        public Series Series { get; set; }

        [Required]
        public int ArtistId { get; set; }

        public Artist Artist { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
