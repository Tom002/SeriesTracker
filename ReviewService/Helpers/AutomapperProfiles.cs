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
            CreateMap<SeriesReviewCreatedEvent, SeriesReview>();
            CreateMap<SeriesReview, SeriesReviewCreatedEvent>();
            CreateMap<EpisodeReviewCreatedEvent, EpisodeReview>();
            CreateMap<EpisodeReview, EpisodeReviewCreatedEvent>();
            CreateMap<SeriesReview, UserSeriesReviewInfoDto>()
                .ForMember(dest => dest.IsReviewedByUser, opt => opt.MapFrom(src => true));
            CreateMap<SeriesReview, SeriesReviewUpdateDto>();
            CreateMap<SeriesReviewUpdateDto, SeriesReview>();
        }
    }
}
