using BioMad_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BioMad_backend.Infrastructure.ServiceConfigurations
{
    public static class HelperServicesConfiguration
    {
        public static IServiceCollection AddHelperServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<TokenService>();
            services.AddSingleton<PasswordService>();
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<ConfirmationService>();
            return services;
        }
    }
}