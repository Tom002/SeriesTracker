using BrowsingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Data
{
    public class Seed
    {
        public static void SeedData(BrowsingDbContext context)
        {
            Category drama, comedy;

            if (!context.Categories.Any())
            {
                drama = new Category
                {
                    CategoryName = "Drama"
                };

                comedy = new Category
                {
                    CategoryName = "Comedy"
                };

                context.Categories.Add(drama);
                context.Categories.Add(comedy);
                context.SaveChanges();
            }

            if (!context.Series.Any())
            {
                var series = new List<Series>
                {
                new Series
                    {
                    Title = "Game of Thrones",
                    CoverImageUrl = "https://m.media-amazon.com/images/M/MV5BYTRiNDQwYzAtMzVlZS00NTI5LWJjYjUtMzkwNTUzMWMxZTllXkEyXkFqcGdeQXVyNDIzMzcwNjc@._V1_UY268_CR7,0,182,268_AL_.jpg",
                    Description = "Nine noble families fight for control over the mythical lands"+
                                    "of Westeros, while an ancient enemy returns after being dormant"+
                                       "for thousands of years.",
                    Episodes = new List<Episode>
                    {
                        new Episode
                        {
                            EpisodeTitle = "Winter is coming",
                            EpisodeNumber = 1,
                            Season = 1,
                            Description = "Eddard Stark is torn between his family and an old friend" +
                                          " when asked to serve at the side of King Robert Baratheon;" +
                                          " Viserys plans to wed his sister to a nomadic warlord in" +
                                          " exchange for an army.",
                            CoverImageUrl = "https://m.media-amazon.com/images/M/MV5BMTQ2NDIzNjEzOV5BMl5BanBnXkFtZTcwOTU2ODg5NA@@._V1_UX224_CR0,0,224,126_AL_.jpg",
                            IsReleased = true,
                            Release = new DateTime(2011,4,18),
                            LengthInMinutes = 52
                        },
                        new Episode
                        {
                            EpisodeTitle = "The Kingsroad",
                            EpisodeNumber = 2,
                            Season = 1,
                            Description = "Eddard Stark is torn between his family and an old friend" +
                                          " when asked to serve at the side of King Robert Baratheon;" +
                                          " Viserys plans to wed his sister to a nomadic warlord in" +
                                          " exchange for an army.",
                            CoverImageUrl = "https://m.media-amazon.com/images/M/MV5BMTQ2NDIzNjEzOV5BMl5BanBnXkFtZTcwOTU2ODg5NA@@._V1_UX224_CR0,0,224,126_AL_.jpg",
                            IsReleased = true,
                            Release = new DateTime(2011,4,25),
                            LengthInMinutes = 52
                        },
                        new Episode
                        {
                            EpisodeTitle = "The North Remembers",
                            EpisodeNumber = 1,
                            Season = 2,
                            Description = "Eddard Stark is torn between his family and an old friend" +
                                          " when asked to serve at the side of King Robert Baratheon;" +
                                          " Viserys plans to wed his sister to a nomadic warlord in" +
                                          " exchange for an army.",
                            CoverImageUrl = "https://m.media-amazon.com/images/M/MV5BMTQ2NDIzNjEzOV5BMl5BanBnXkFtZTcwOTU2ODg5NA@@._V1_UX224_CR0,0,224,126_AL_.jpg",
                            IsReleased = true,
                            Release = new DateTime(2012,4,2),
                            LengthInMinutes = 52
                        },
                        new Episode
                        {
                            EpisodeTitle = "The Night Lands",
                            EpisodeNumber = 2,
                            Season = 2,
                            Description = "Eddard Stark is torn between his family and an old friend" +
                                          " when asked to serve at the side of King Robert Baratheon;" +
                                          " Viserys plans to wed his sister to a nomadic warlord in" +
                                          " exchange for an army.",
                            CoverImageUrl = "https://m.media-amazon.com/images/M/MV5BMTQ2NDIzNjEzOV5BMl5BanBnXkFtZTcwOTU2ODg5NA@@._V1_UX224_CR0,0,224,126_AL_.jpg",
                            IsReleased = true,
                            Release = new DateTime(2012,4,9),
                            LengthInMinutes = 52
                        },

                    },
                }


                };
                context.Series.AddRange(series);
                context.SaveChanges();
            }

        }
    }
}
