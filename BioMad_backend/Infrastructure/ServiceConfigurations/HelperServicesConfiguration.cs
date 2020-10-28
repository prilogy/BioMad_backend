using BioMad_backend.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BioMad_backend.Infrastructure.ServiceConfigurations
{
    public static class HelperServicesConfiguration
    {
        public static IServiceCollection AddHelperServices(this IServiceCollection services)
        {
            services.AddSingleton<PasswordService>();
            services.AddScoped<UserService>();

            return services;
        }
    }
}