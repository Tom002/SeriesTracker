using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Models
{
    public class Artist
    {
        [Required]
        public int ArtistId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? DeathDate { get; set; }

        [MaxLength(200)]
        public string City { get; set; }

        [MaxLength(1000)]
        public string About { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<SeriesWriter> WriterOf { get; set; } = new List<SeriesWriter>();

        public ICollection<SeriesActor> AppearedIn { get; set; } = new List<SeriesActor>();
    }
}
