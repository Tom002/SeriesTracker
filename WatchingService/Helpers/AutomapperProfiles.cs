using AutoMapper;
using Common.Events;
using System;
using WatchingService.Dto;
using WatchingService.Models;

namespace WatchingService.Helpers
{
    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<UserCreatedEvent, Viewer>()
                .ForMember(dest => dest.ViewerId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<SeriesCreatedEvent, Series>()
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CoverImageUrl))
                .ForMember(dest => dest.EndYear, opt => opt.MapFrom(src => src.EndYear))
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(src => src.SeriesId))
                .ForMember(dest => dest.StartYear, opt => opt.MapFrom(src => src.StartYear))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<EpisodeCreatedEvent, Episode>()
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.CoverImageUrl))
                .ForMember(dest => dest.EpisodeId, opt => opt.MapFrom(src => src.EpisodeId))
                .ForMember(dest => dest.EpisodeNumber, opt => opt.MapFrom(src => src.EpisodeNumber))
                .ForMember(dest => dest.EpisodeTitle, opt => opt.MapFrom(src => src.EpisodeTitle))
                .ForMember(dest => dest.Release, opt => opt.MapFrom(src => src.Release))
                .ForMember(dest => dest.Season, opt => opt.MapFrom(src => src.Season))
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(src => src.SeriesId));

            CreateMap<SeriesWatched, SeriesWatchedListDto>()
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.Series.CoverImageUrl))
                .ForMember(dest => dest.EndYear, opt => opt.MapFrom(src => src.Series.EndYear))
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(src => src.Series.SeriesId))
                .ForMember(dest => dest.StartYear, opt => opt.MapFrom(src => src.Series.StartYear))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Series.Title));

            CreateMap<SeriesWatched, SeriesWatchedDeletedEvent>()
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(src => src.SeriesId))
                .ForMember(dest => dest.ViewerId, opt => opt.MapFrom(src => src.ViewerId));

            CreateMap<SeriesWatched, SeriesWatchedEvent>()
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(src => src.SeriesId))
                .ForMember(dest => dest.ViewerId, opt => opt.MapFrom(src => src.ViewerId));

            CreateMap<EpisodeWatched, EpisodeWatchedEvent>()
                .ForMember(dest => dest.EpisodeId, opt => opt.MapFrom(src => src.EpisodeId))
                .ForMember(dest => dest.ViewerId, opt => opt.MapFrom(src => src.ViewerId));

            CreateMap<EpisodeWatched, EpisodeWatchedDeletedEvent>()
                .ForMember(dest => dest.EpisodeId, opt => opt.MapFrom(src => src.EpisodeId))
                .ForMember(dest => dest.ViewerId, opt => opt.MapFrom(src => src.ViewerId));

            CreateMap<SeriesLiked, SeriesLikedListDto>()
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src => src.Series.CoverImageUrl))
                .ForMember(dest => dest.EndYear, opt => opt.MapFrom(src => src.Series.EndYear))
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(src => src.Series.SeriesId))
                .ForMember(dest => dest.StartYear, opt => opt.MapFrom(src => src.Series.StartYear))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Series.Title));

            CreateMap<SeriesLiked, SeriesLikedEvent>()
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(src => src.SeriesId))
                .ForMember(dest => dest.ViewerId, opt => opt.MapFrom(src => src.ViewerId));

            CreateMap<SeriesLiked, SeriesLikedDeletedEvent>()
                .ForMember(dest => dest.SeriesId, opt => opt.MapFrom(src => src.SeriesId))
                .ForMember(dest => dest.ViewerId, opt => opt.MapFrom(src => src.ViewerId));

        }
    }
}
