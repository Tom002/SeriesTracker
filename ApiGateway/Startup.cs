using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Kubernetes;

namespace ApiGateway
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
            var authenticationProviderKey = "IdentityServerAuthentication";

            // Default value, docker compose url
            string identityUrl = "http://identityservice";
            
            if(!String.IsNullOrEmpty(Configuration["IDENTITY_URL"]))
            {
                identityUrl = Configuration["IDENTITY_URL"];
            }

            Action<IdentityServerAuthenticationOptions> options = o =>
            {
                o.RequireHttpsMetadata = false;
                o.Authority = identityUrl;
                o.ApiName = "gateway";
                o.SupportedTokens = SupportedTokens.Both;
            };

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddAuthentication()
                    .AddIdentityServerAuthentication(authenticationProviderKey, options);
            services.AddOcelot()
                .AddKubernetes();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("default");

            //app.UseAuthentication();
            app.UseOcelot().Wait();
        }
    }
}
