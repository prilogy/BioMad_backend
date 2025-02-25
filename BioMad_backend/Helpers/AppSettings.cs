﻿using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BioMad_backend.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string JwtIssuer { get; set; }
        
        public string BaseUrl { get; set; }
        
        public string GoogleClientId { get; set; }

        public string EmailSenderSmtp { get; set; }
        public string EmailSenderLogin { get; set; }
        public string EmailSenderPassword { get; set; }
        public string EmailSenderName { get; set; }
        
        public string AdminSecret { get; set; }
        
        public const string Key = "AppSettings";
    } 

    public static class AppSettingsExtensions
    {
        public static AppSettings ConfigureAppSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection(AppSettings.Key);
            services.Configure<AppSettings>(appSettingsSection);
            return configuration.GetSection(AppSettings.Key).Get<AppSettings>();
        }

        public static byte[] GetHashedKey(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection(AppSettings.Key).Get<AppSettings>();
            return Encoding.ASCII.GetBytes(appSettings.Secret);
        }
    }
}