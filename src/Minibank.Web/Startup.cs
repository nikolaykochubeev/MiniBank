using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Minibank.Core;
using Minibank.Data;
using Minibank.Web.HostedServices;
using Minibank.Web.Middlewares;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;


namespace Minibank.Web
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
            services
                .AddMvc()
                .AddJsonOptions(opts =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                });
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Minibank.Web", Version = "v1" });
                
                options.AddSecurityDefinition("oauth2", 
                    new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(Configuration["TokenUri"]),
                            Scopes = new Dictionary<string, string>()
                        }
                    }
                });
                
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = SecuritySchemeType.OAuth2.GetDisplayName(),
                            }
                        },
                        new List<string>()
                    }
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Audience = Configuration["AudienceType"];
                options.Authority = Configuration["AuthorityUri"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = false,
                    ValidateAudience = false
                };
            });
            
            services.AddHostedService<MigrationHostedService>();
            
            services
                .AddData(Configuration)
                .AddCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minibank.Web v1"));
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<MinibankExceptionMiddleware>();
            app.UseMiddleware<CustomAuthenticationMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}