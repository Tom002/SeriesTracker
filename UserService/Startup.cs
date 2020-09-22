using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProfileService.Data;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using ProfileService.Helpers;

namespace ProfileService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

            services.AddDbContext<UserDbContext>(opt =>
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
            services.AddCap(x =>
            {
                x.UseEntityFramework<UserDbContext>();
                x.UseRabbitMQ(conf =>
                {
                    conf.HostName = Configuration["RabbitMQConfig:Hostname"];
                    conf.UserName = Configuration["RabbitMQConfig:UserName"];
                    conf.Password = rabbitPassword;
                    conf.Port = int.Parse(Configuration["RabbitMQConfig:Port"]);
                });
            });
            services.AddControllers();

            services.AddMvcCore(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddAuthorization();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityUrl;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "users";
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
