using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BioMad_backend.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
    }
  
    public static class AppSettingsExtensions {
        public static byte[] ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            return key;
        }
    }
}