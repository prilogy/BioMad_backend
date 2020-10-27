using BioMad_backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BioMad_backend.Infrastructure.ServiceConfigurations
{
    public static class DbServiceConfiguration
    {
        public static IServiceCollection ConfigureDbService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                var connect = configuration.GetConnectionString("Default");
                options.UseLazyLoadingProxies();
                options.UseNpgsql(connect);
            });

            return services;
        }
    }
}