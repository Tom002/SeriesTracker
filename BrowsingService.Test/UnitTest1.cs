using AutoMapper;
using BrowsingService.Data;
using BrowsingService.Helpers;
using BrowsingService.Models;
using BrowsingService.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace BrowsingService.Test
{
    public class UnitTest1
    {
        public BrowsingDbContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder<BrowsingDbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new BrowsingDbContext(builder.Options);

            return context;
        }

        public IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperProfiles>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        }

        public void SeedDatabase(BrowsingDbContext context)
        {
            context.Categories.Add(
                new Models.Category
                {
                    CategoryId = 1,
                    CategoryName = "Drama"
                });

            context.Categories.Add(
                new Models.Category
                {
                    CategoryId = 2,
                    CategoryName = "Comedy"
                });

            context.Categories.Add(
                new Models.Category
                {
                    CategoryId = 3,
                    CategoryName = "Action"
                });


            context.Series.Add(
                new Models.Series
                {
                    SeriesId = 1,
                    Title = "Breaking Bad",
                    Categories = new List<SeriesCategory>
                    {
                        new SeriesCategory
                        {
                            CategoryId = 1
                        }
                    },
                    Description = "A breaking Bad egy sorozat",
                    Episodes = new List<Episode>
                    {
                        new Episode
                        {
                            EpisodeId = 1,
                            EpisodeNumber = 1,
                            Season = 1,
                            EpisodeTitle = "Elsõ rész",
                        }
                    }
                });

            context.SaveChanges();
        }

        [Fact]
        public async void GetCategories()
        {
            var mapper = GetMapper();
            using (var context = GetDbContext())
            {
                SeedDatabase(context);
                var service = new SeriesService(context, mapper);
                var categories = await service.GetCategories();

                Assert.Equal(3, categories.Count);
            }
        }
    }
}
