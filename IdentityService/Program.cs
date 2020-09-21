using System;
using DotNetCore.CAP;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var usermanager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var capBus = services.GetRequiredService<ICapPublisher>();
                    context.Database.EnsureDeleted();
                    context.Database.Migrate();                    
                    Seed.SeedData(usermanager, capBus, context).Wait();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("config/appsettings.json", optional:true, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
