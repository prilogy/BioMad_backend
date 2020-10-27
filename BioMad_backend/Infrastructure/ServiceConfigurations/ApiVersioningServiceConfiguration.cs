using BioMad_backend.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BioMad_backend.Infrastructure.ServiceConfigurations
{
    public static class ApiVersioningServiceConfiguration
    {
        public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning();
            return services;
        }
    }
}