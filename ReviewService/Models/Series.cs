using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Models
{
    public class Series
    {
        [Required]
        public int SeriesId { get; set; }

        public ICollection<SeriesReview> Reviews { get; set; }
    }
}
