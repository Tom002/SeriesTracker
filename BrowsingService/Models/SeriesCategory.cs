using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Models
{
    public class SeriesCategory
    {
        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        public int SeriesId { get; set; }

        public Series Series { get; set; }
    }
}
