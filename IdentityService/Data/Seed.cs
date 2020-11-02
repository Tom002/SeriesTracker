using Common.Events;
using DotNetCore.CAP;
using IdentityService.Helpers;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Data
{
    public class Seed
    {
        public static async Task SeedData(UserManager<ApplicationUser> userManager, ICapPublisher capBus, ApplicationDbContext context)
        {
            if (!userManager.Users.Any())
            {
                using (var trans = context.Database.BeginTransaction(capBus, autoCommit: true))
                {
                    var identityUser = new ApplicationUser
                    {
                        UserName = "bob@test.com",
                        Email = "bob@test.com"
                    };

                    var result = await userManager.CreateAsync(identityUser, "Password123");

                    var userEvent = new UserCreatedEvent
                    {
                        UserId = identityUser.Id,
                        Name = "Bob",
                        City = "Budapest",
                        Email = "bob@test.com",
                        BirthDate = new DateTime(1997, 10, 23),
                        ProfileImageUrl = "https://images.dog.ceo/breeds/husky/n02110185_6775.jpg"
                    };

                    await capBus.SendEventAsync("identityservice.user.created", userEvent);
                }
            }
        }
    }
}
