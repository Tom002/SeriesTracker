using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Dto
{
    public class WatchEpisodeDto
    {
        [Required]
        public string ViewerId { get; set; }

        [Required]
        public int EpisodeId { get; set; }

        [Required]
        public DateTime WatchingDate { get; set; }

        [Required]
        public bool AddToDiary { get; set; }
    }
}
