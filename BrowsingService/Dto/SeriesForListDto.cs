using BrowsingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Dto
{
    public class SeriesForListDto
    {
        public int SeriesId { get; set; }

        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }

        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

        public double AverageRating { get; set; }
    }
}
