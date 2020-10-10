using System.Collections.Generic;

namespace BrowsingService.Dto
{
    public class SeriesDetailsDto
    {
        public int SeriesId { get; set; }

        public string Title { get; set; }

        public string CoverImageUrl { get; set; }

        public string Description { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }

        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

        public List<ActorWithRoleDto> Cast { get; set; } = new List<ActorWithRoleDto>();

        public List<WriterDto> Writers { get; set; } = new List<WriterDto>();

        //public List<SeriesReviewDto> Reviews { get; set; } = new List<SeriesReviewDto>();

        public double? RatingAverage { get; set; }

        public int RatingsCount { get; set; }

        public List<int> SeasonNumbers { get; set; } = new List<int>();
    }
}
