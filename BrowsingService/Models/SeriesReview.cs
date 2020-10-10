using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Models
{
    public class SeriesReview
    {
        [Required]
        public int SeriesId { get; set; }
        public Series Series { get; set; }
        [Required]
        public string ReviewerId { get; set; }
        [Range(1, 5)]
        public int? Rating { get; set; }
    }
}
