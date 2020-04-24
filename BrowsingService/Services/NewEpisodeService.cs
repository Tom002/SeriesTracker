using AutoMapper;
using AutoMapper.Configuration;
using BrowsingService.Data;
using BrowsingService.Models;
using Common.Events;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BrowsingService.Services
{
    public class NewEpisodeService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public NewEpisodeService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.DoWork(stoppingToken);
            }
        }
    }
}
