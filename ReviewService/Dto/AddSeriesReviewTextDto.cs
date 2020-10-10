using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Dto
{
    public class AddSeriesReviewTextDto
    {
        public string ReviewTitle { get; set; }
        [Required]
        public string ReviewText { get; set; }
    }
}
