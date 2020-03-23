using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Models
{
    public class SeriesFollowed
    {
        [Required]
        public int SeriesId { get; set; }

        public Series Series { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
