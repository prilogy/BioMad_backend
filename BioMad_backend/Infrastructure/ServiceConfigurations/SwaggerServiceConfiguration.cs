using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BioMad_backend.Infrastructure.ServiceConfigurations
{
    public static class SwaggerServiceConfiguration
    {
        public const string TitleBase = "BioMad API";
        public const string Description = "Web API documentation for BioMad application.";

        public static OpenApiLicense License = new OpenApiLicense
        {
            Name = "MIT"
        };

        public static OpenApiContact Contact = new OpenApiContact()
        {
            Name = "Artyom Lukyanov",
            Email = "artglz63@gmail.com"
        };

        public static List<OpenApiInfo> Versions = new List<OpenApiInfo>
        {
            new OpenApiInfo
            {
                Version = "v1",
                Title = TitleBase + " v1",
                Description = Description,
                License = License,
                Contact = Contact
            },
            new OpenApiInfo
            {
                Version = "v2",
                Title = TitleBase + " v2",
                Description = Description,
                License = License,
                Contact = Contact
            }
        };

        public static IServiceCollection ConfigureSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                foreach (var v in Versions)
                {
                    config.SwaggerDoc(v.Version, v);
                }

                config.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });

                config.OperationFilter<AuthenticationRequirementsOperationFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static void UseSwaggerWithCustomConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                foreach (var v in Versions)
                {
                    config.SwaggerEndpoint($"/swagger/{v.Version}/swagger.json", v.Title);
                }
            });
        }

        private class AuthenticationRequirementsOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();

                var scheme = new OpenApiSecurityScheme
                    { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" } };
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [scheme] = new List<string>()
                });
            }
        }
    }
}