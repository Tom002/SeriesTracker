using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Models
{
    public class Series
    {
        [MaxLength(200)]
        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        public int StartYear { get; set; }

        public int? EndYear { get; set; }

        [Required]
        public int SeriesId { get; set; }

        public ICollection<SeriesReview> Reviews { get; set; }
    }
}
