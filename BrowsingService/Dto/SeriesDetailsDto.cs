using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Dto
{
    public class SeriesDetailsDto
    {
        [Required]
        public int SeriesId { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }

        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

        public List<ActorWithRoleDto> Cast { get; set; } = new List<ActorWithRoleDto>();

        public List<WriterDto> Writers { get; set; } = new List<WriterDto>();

        public double RatingAverage { get; set; }

        public int RatingsCount { get; set; }

        public List<int> SeasonNumbers { get; set; } = new List<int>();
    }
}
