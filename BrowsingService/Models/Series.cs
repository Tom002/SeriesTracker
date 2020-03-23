using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Models
{
    public class Series
    {
        [Required]
        public int SeriesId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public ICollection<Episode> Episodes { get; set; } = new List<Episode>();

        public ICollection<SeriesWriter> Writers { get; set; } = new List<SeriesWriter>();

        public ICollection<SeriesActor> Cast { get; set; } = new List<SeriesActor>();

        public ICollection<SeriesCategory> Categories { get; set; } = new List<SeriesCategory>();

        public ICollection<SeriesReview> Reviews { get; set; } = new List<SeriesReview>();
    }
}
