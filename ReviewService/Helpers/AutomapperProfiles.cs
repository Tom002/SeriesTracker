using AutoMapper;
using ReviewService.Dto;
using ReviewService.Models;
using Common.Events;
using System;

namespace ReviewService.Helpers
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<CreateSeriesReviewDto, SeriesReview>();
            CreateMap<EpisodeReviewManipulationDto, EpisodeReview>();
            CreateMap<SeriesReviewCreatedEvent, SeriesReview>();
            CreateMap<SeriesReview, SeriesReviewCreatedEvent>();
            CreateMap<SeriesReview, SeriesReviewUpdatedEvent>();
            CreateMap<EpisodeReviewCreatedEvent, EpisodeReview>();
            CreateMap<EpisodeReview, EpisodeReviewCreatedEvent>();
            CreateMap<SeriesReview, UserSeriesReviewInfoDto>()
                .ForMember(dest => dest.IsReviewedByUser, opt => opt.MapFrom(src => true));
            CreateMap<SeriesReview, SeriesReviewUpdateDto>();
            CreateMap<SeriesReviewUpdateDto, SeriesReview>();

            CreateMap<UserCreatedEvent, Reviewer>()
                .ForMember(dest => dest.ReviewerId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name));

            CreateMap<EpisodeCreatedEvent, Episode>()
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CoverImageUrl))
                .ForMember(dest => dest.EpisodeId, opt => opt.MapFrom(src => src.EpisodeId))
                .ForMember(dest => dest.EpisodeNumber, opt => opt.MapFrom(src => src.EpisodeNumber))
                .ForMember(dest => dest.EpisodeTitle, opt => opt.MapFrom(src => src.EpisodeTitle))
                .ForMember(dest => dest.Release, opt => opt.MapFrom(src => src.Release))
                .ForMember(dest => dest.Season, opt => opt.MapFrom(src => src.Season))
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(src => src.SeriesId));

            CreateMap<SeriesCreatedEvent, Series>()
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CoverImageUrl))
                .ForMember(dest => dest.EndYear, opt => opt.MapFrom(src => src.EndYear))
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(src => src.SeriesId))
                .ForMember(dest => dest.StartYear, opt => opt.MapFrom(src => src.StartYear))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<AddSeriesReviewRatingDto, SeriesReview>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<AddSeriesReviewTextDto, SeriesReview>()
                .ForMember(dest => dest.ReviewText, opt => opt.MapFrom(src => src.ReviewText))
                .ForMember(dest => dest.ReviewTitle, opt => opt.MapFrom(src => src.ReviewTitle))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<SeriesReview, SeriesReviewListDto>()
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.Reviewer.UserName));

            CreateMap<SeriesReview, UserSeriesReviewListDto>()
                .ForMember(dest => dest.SeriesTitle, opt => opt.MapFrom(src => src.Series.Title));
        }       
    }
}
