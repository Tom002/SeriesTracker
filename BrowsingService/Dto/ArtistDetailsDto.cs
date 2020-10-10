using System;
using System.Collections.Generic;

namespace BrowsingService.Dto
{
    public class ArtistDetailsDto
    {
        public int ArtistId { get; set; }

        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? DeathDate { get; set; }

        public string City { get; set; }

        public string About { get; set; }

        public string ImageUrl { get; set; }

        public List<WriterOfSeriesDto> WriterOf { get; set; } = new List<WriterOfSeriesDto>();

        public List<ActorInSeriesDto> AppearedIn { get; set; } = new List<ActorInSeriesDto>();
    }
}
