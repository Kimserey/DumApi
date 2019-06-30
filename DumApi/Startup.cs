using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace DumApi
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
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .Configure<JwtOptions>(Configuration.GetSection("Jwt"));

            services
                .AddTransient<IJwtToken, JwtToken>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "sub",
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Configuration["Jwt:Key"]))
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/openapi/openapi.yml", "Value API");
                c.SwaggerEndpoint("/sampleopenapi/openapi.yml", "Sample Value API");
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/openapi",
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "openapi")
                ),
                ContentTypeProvider = new FileExtensionContentTypeProvider(
                    new Dictionary<string, string>()
                    {
                        [".json"] = "application/json",
                        [".yml"] = "application/yml"
                    }
                ),
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
                }
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/sampleopenapi",
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "sampleopenapi")
                ),
                ContentTypeProvider = new FileExtensionContentTypeProvider(
                    new Dictionary<string, string>()
                    {
                        [".json"] = "application/json",
                        [".yml"] = "application/yml"
                    }
                ),
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
                }
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
