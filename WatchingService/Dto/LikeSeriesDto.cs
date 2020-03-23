using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WatchingService.Dto
{
    public class LikeSeriesDto
    {
        [Required]
        public string ViewerId { get; set; }

        [Required]
        public int SeriesId { get; set; }
    }
}
