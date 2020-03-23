using AutoMapper;
using ReviewService.Dto;
using ReviewService.Models;
using Common.Events;

namespace ReviewService.Helpers
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<CreateSeriesReviewDto, SeriesReview>();
            CreateMap<CreateEpisodeReviewDto, EpisodeReview>();
            CreateMap<SeriesReview, SeriesReviewCreatedEvent>();
            CreateMap<EpisodeReview, EpisodeReviewCreatedEvent>();
        }
    }
}
