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
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(300)]
        public string About { get; set; }

        public ICollection<SeriesWriter> WriterOf { get; set; } = new List<SeriesWriter>();

        public ICollection<SeriesActor> AppearedIn { get; set; } = new List<SeriesActor>();
    }
}
