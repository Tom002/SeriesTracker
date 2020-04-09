using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Models
{
    public class Category
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CategoryName { get; set; }

        public ICollection<SeriesCategory> Series { get; set; } = new List<SeriesCategory>();
    }
}
