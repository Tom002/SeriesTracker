using AutoMapper;
using Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System;
using WatchingService.Data;
using WatchingService.Helpers;
using WatchingService.Helpers.RequestContext;
using WatchingService.Interfaces;
using WatchingService.Services;

namespace WatchingService
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
            // Default value, docker compose url
            string identityUrl = "http://identityservice";

            if (!String.IsNullOrEmpty(Configuration["IDENTITY_URL"]))
            {
                identityUrl = Configuration["IDENTITY_URL"];
            }

            string rabbitPassword;
            if (String.IsNullOrEmpty(Configuration["RABBITMQ_PASSWORD"]))
            {
                rabbitPassword = Configuration["RabbitMQConfig:Password"];
            }
            else
            {
                rabbitPassword = Configuration["RABBITMQ_PASSWORD"];
            }

            services.AddMvcCore(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddAuthorization();

            services.AddDbContext<WatchingDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("AzureSqlConnection"));
            });

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityUrl;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "watching";
                });
            IdentityModelEventSource.ShowPII = true;

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddTransient<ISubscriberService, SubscriberService>();
            services.AddCap(x =>
            {
                x.UseEntityFramework<WatchingDbContext>();
                x.UseRabbitMQ(conf =>
                {
                    conf.HostName = Configuration["RabbitMQConfig:Hostname"];
                    conf.UserName = Configuration["RabbitMQConfig:UserName"];
                    conf.Password = Configuration["RabbitMQConfig:Password"];
                    conf.Port = int.Parse(Configuration["RabbitMQConfig:Port"]);
                });
            });
            services.AddHttpContextAccessor();
            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:InstrumentationKey"]);
            services.AddScoped<IWatchingService, Services.WatchingService>();
            services.AddScoped<IRequestContext, HttpRequestContext>();
            services.AddScoped<IMessageTracker, MessageTracker>();
            services.AddAutoMapper(typeof(AutomapperProfiles));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("default");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
