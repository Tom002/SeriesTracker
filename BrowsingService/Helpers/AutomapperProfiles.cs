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
