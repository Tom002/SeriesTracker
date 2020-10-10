using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Dto
{
    public class EpisodeReviewManipulationDto
    {
        [Required]        
        [Range(1,5)]
        public int Rating { get; set; }
    }
}
