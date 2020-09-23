using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BrowsingService.Data;
using BrowsingService.Helpers;
using BrowsingService.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BrowsingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<BrowsingDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                //sqlServerOptionsAction: sqlOptions =>
                //{
                //    sqlOptions.EnableRetryOnFailure(
                //    maxRetryCount: 10,
                //    maxRetryDelay: TimeSpan.FromSeconds(30),
                //    errorNumbersToAdd: null);
                //});
            });
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddCap(x =>
            {
                x.UseEntityFramework<BrowsingDbContext>();
                x.UseRabbitMQ(conf =>
                {
                    conf.HostName = Configuration["RabbitMQConfig:Hostname"];
                    conf.UserName = Configuration["RabbitMQConfig:UserName"];
                    conf.Password = Configuration["RabbitMQConfig:Password"];
                    conf.Port = int.Parse(Configuration["RabbitMQConfig:Port"]);
                });
            });
            services.AddAutoMapper(typeof(AutomapperProfiles));
            services.AddScoped<IMovieDbClient, MovieDbClient>();
            services.AddScoped<IScopedProcessingService, GetNewEpisodesTask>();
            services.AddHostedService<NewEpisodeService>();
            //var hcBuilder = services.AddHealthChecks();
            //hcBuilder
            //    .AddSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection"),
            //        name: "CatalogDB-check",
            //        tags: new string[] { "catalogdb" });
            //hcBuilder
            //        .AddRabbitMQ(
            //            $"amqp://" +
            //            $"{Configuration["RabbitMQConfig:UserName"]}:" +
            //            $"{rabbitPassword}@" +
            //            $"{Configuration["RabbitMQConfig:Hostname"]}:" +
            //            $"{Configuration["RabbitMQConfig: Port"]}/",
            //            name: "catalog-rabbitmqbus-check",
            //            tags: new string[] { "rabbitmqbus" });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation(Configuration["RabbitMQConfig:Password"]);
            logger.LogInformation(Configuration["RabbitMQConfig:Password"]);
            logger.LogInformation(Configuration["RabbitMQConfig:Password"]);
            logger.LogInformation(Configuration["RabbitMQConfig:Password"]);
            logger.LogInformation(Configuration["RabbitMQConfig:Password"]);
            if (env.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened, try again later");
                    });
                });
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("default");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("/health/readiness", new HealthCheckOptions()
                //{
                //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                //});
                //endpoints.MapHealthChecks("/health/liveness", new HealthCheckOptions()
                //{
                //    Predicate = _ => false,
                //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                //});
                endpoints.MapControllers();
            });
        }
    }
}
