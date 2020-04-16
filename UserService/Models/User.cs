using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Models
{
    public class User
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string City { get; set; }
        [MaxLength(400)]
        public string About { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
