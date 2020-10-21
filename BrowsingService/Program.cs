using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BrowsingService.Data;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Identity;

namespace BrowsingService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<BrowsingDbContext>();
                    var capBus = services.GetRequiredService<ICapPublisher>();
                    var mapper = services.GetRequiredService<IMapper>();
                    //context.Database.EnsureDeleted();
                    //context.Database.Migrate();
                    await Seed.SeedData(context, capBus, mapper);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(
                    builder =>
                    {
                        builder.AddApplicationInsights("b557d7f2-f8cb-4a42-9445-e9890611631d");
                        builder.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider>
                                         ("", LogLevel.Information);
                    }
                )
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
