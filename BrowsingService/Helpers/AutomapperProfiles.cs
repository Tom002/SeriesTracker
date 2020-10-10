using AutoMapper;
using BrowsingService.Dto;
using BrowsingService.Models;
using BrowsingService.Services.ApiResponseModels;
using Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Series, SeriesCreatedEvent>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Episode, EpisodeCreatedEvent>();
            CreateMap<Episode, EpisodeDto>();
            CreateMap<Artist, ArtistForListDto>().ConvertUsing<ArtistToDtoConverter>();
            CreateMap<PersonApiResponse, Artist>().ConvertUsing<PersonToArtistConverter>();
            CreateMap<SeriesActor, ActorWithRoleDto>().ConvertUsing<SeriesActorToDtoConverter>();
            CreateMap<Genre, Category>()
                .ForMember(c => c.CategoryId, opt => opt.MapFrom(g => g.id))
                .ForMember(c => c.CategoryName, opt => opt.MapFrom(g => g.name));
            CreateMap<SeriesReview, SeriesReviewDto>();
            CreateMap<SeriesReviewCreatedEvent, SeriesReview>();

            CreateMap<SeriesReviewUpdatedEvent, SeriesReview>()
                .ForMember(c => c.Rating, opt => opt.MapFrom(g => g.Rating))
                .ForMember(c => c.ReviewerId, opt => opt.Ignore())
                .ForMember(c => c.SeriesId, opt => opt.Ignore());

            CreateMap<SeriesCategory, CategoryDto>()
                .ForMember(c => c.CategoryId, opt => opt.MapFrom(g => g.CategoryId))
                .ForMember(c => c.CategoryName, opt => opt.MapFrom(g => g.Category.CategoryName));

            CreateMap<Series, SeriesForListDto>()
                .ForMember(c => c.AverageRating, opt => opt.MapFrom(g => g.Reviews.Select(r => r.Rating).Average()))
                .ForMember(c => c.Categories, opt => opt.MapFrom(g => g.Categories))
                .ForMember(c => c.CoverImageUrl, opt => opt.MapFrom(g => g.CoverImageUrl))
                .ForMember(c => c.EndYear, opt => opt.MapFrom(g => g.EndYear))
                .ForMember(c => c.SeriesId, opt => opt.MapFrom(g => g.SeriesId))
                .ForMember(c => c.StartYear, opt => opt.MapFrom(g => g.StartYear))
                .ForMember(c => c.Title, opt => opt.MapFrom(g => g.Title));

            CreateMap<Episode, EpisodeDto>()
                .ForMember(c => c.CoverImageUrl, opt => opt.MapFrom(g => g.CoverImageUrl))
                .ForMember(c => c.Description, opt => opt.MapFrom(g => g.Description))
                .ForMember(c => c.EpisodeId, opt => opt.MapFrom(g => g.EpisodeId))
                .ForMember(c => c.EpisodeNumber, opt => opt.MapFrom(g => g.EpisodeNumber))
                .ForMember(c => c.EpisodeTitle, opt => opt.MapFrom(g => g.EpisodeTitle))
                .ForMember(c => c.IsReleased, opt => opt.MapFrom(g => g.Release.HasValue))
                .ForMember(c => c.LengthInMinutes, opt => opt.MapFrom(g => g.LengthInMinutes))
                .ForMember(c => c.Release, opt => opt.MapFrom(g => g.Release))
                .ForMember(c => c.Season, opt => opt.MapFrom(g => g.Season))
                .ForMember(c => c.SeriesId, opt => opt.MapFrom(g => g.SeriesId));

            CreateMap<SeriesActor, ActorInSeriesDto>()
                .ForMember(c => c.CoverImageUrl, opt => opt.MapFrom(g => g.Series.CoverImageUrl))
                .ForMember(c => c.RoleName, opt => opt.MapFrom(g => g.RoleName))
                .ForMember(c => c.SeriesId, opt => opt.MapFrom(g => g.SeriesId))
                .ForMember(c => c.Title, opt => opt.MapFrom(g => g.Series.Title));

            CreateMap<SeriesWriter, WriterOfSeriesDto>()
                .ForMember(c => c.CoverImageUrl, opt => opt.MapFrom(g => g.Series.CoverImageUrl))
                .ForMember(c => c.SeriesId, opt => opt.MapFrom(g => g.SeriesId))
                .ForMember(c => c.Title, opt => opt.MapFrom(g => g.Series.Title));

            CreateMap<Artist, ArtistDetailsDto>()
                .ForMember(c => c.About, opt => opt.MapFrom(g => g.About))
                .ForMember(c => c.AppearedIn, opt => opt.MapFrom(g => g.AppearedIn))
                .ForMember(c => c.ArtistId, opt => opt.MapFrom(g => g.ArtistId))
                .ForMember(c => c.BirthDate, opt => opt.MapFrom(g => g.BirthDate))
                .ForMember(c => c.City, opt => opt.MapFrom(g => g.City))
                .ForMember(c => c.DeathDate, opt => opt.MapFrom(g => g.DeathDate))
                .ForMember(c => c.ImageUrl, opt => opt.MapFrom(g => g.ImageUrl))
                .ForMember(c => c.Name, opt => opt.MapFrom(g => g.Name))
                .ForMember(c => c.WriterOf, opt => opt.MapFrom(g => g.WriterOf));
        }

        public class PersonToArtistConverter : ITypeConverter<PersonApiResponse, Artist>
        {
            const string imageBasePath = "https://image.tmdb.org/t/p/original";
            public Artist Convert(PersonApiResponse source, Artist destination, ResolutionContext context)
            {
                return new Artist
                {
                    ArtistId = source.id,
                    About = source.biography.Substring(0, Math.Min(1000, source.biography.Length)),
                    ImageUrl = imageBasePath + source.profile_path,
                    BirthDate = source.birthday,
                    DeathDate = source.deathday,
                    City = source.place_of_birth == null ? "" : source.place_of_birth.Substring(0, Math.Min(200, source.place_of_birth.Length)), 
                    Name = source.name.Substring(0, Math.Min(200, source.name.Length))
                };
            }
        }

        public class ArtistToDtoConverter : ITypeConverter<Artist, ArtistForListDto>
        {
            public ArtistForListDto Convert(Artist source, ArtistForListDto destination, ResolutionContext context)
            {
                return new ArtistForListDto
                {
                    ArtistId = source.ArtistId,
                    BirthDate = source.BirthDate,
                    DeathDate = source.DeathDate,
                    AppearedInCount = source.AppearedIn.Count(),
                    WriterOfCount = source.WriterOf.Count(),
                    City = source.City,
                    Name = source.Name,
                    ImageUrl = source.ImageUrl
                };
            }
        }

        public class SeriesActorToDtoConverter : ITypeConverter<SeriesActor, ActorWithRoleDto>
        {
            public ActorWithRoleDto Convert(SeriesActor source, ActorWithRoleDto destination, ResolutionContext context)
            {
                return new ActorWithRoleDto
                {
                    ArtistId = source.ArtistId,
                    ImageUrl = source.Artist.ImageUrl,
                    Name = source.Artist.Name,
                    RoleName = source.RoleName,
                    Order = source.Order
                };
            }
        }
    }
}
