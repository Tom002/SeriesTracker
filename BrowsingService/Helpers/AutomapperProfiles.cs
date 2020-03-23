using AutoMapper;
using BrowsingService.Models;
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
            CreateMap<SeriesReviewCreatedEvent, SeriesReview>();
            CreateMap<EpisodeReviewCreatedEvent, EpisodeReview>();
        }
    }
}
