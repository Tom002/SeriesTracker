using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Models
{
    public class EpisodeCalendar
    {
        [Required]
        public int EpisodeId { get; set; }

        public Episode Episode { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
