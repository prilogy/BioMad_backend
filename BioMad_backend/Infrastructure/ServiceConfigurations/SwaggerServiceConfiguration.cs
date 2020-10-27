using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BioMad_backend.Infrastructure.ServiceConfigurations
{
    public static class SwaggerServiceConfiguration
    {
        public static IServiceCollection ConfigureSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "v1 API",
                    Description = "v1 API Description"
                }));

            return services;
        }
    }
}