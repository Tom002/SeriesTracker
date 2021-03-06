﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Dto
{
    public class CreateSeriesReviewDto
    {
        [Required]
        public string ReviewerId { get; set; }

        [Required]
        public int SeriesId { get; set; }

        public string ReviewTitle { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
    }
}
