using AutoMapper;
using Common.Events;
using ProfileService.Dto;
using ProfileService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<UserCreatedEvent, User>();
            CreateMap<User, UserProfileDto>();
        }
    }
}
