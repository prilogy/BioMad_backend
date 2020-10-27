using BioMad_backend.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BioMad_backend.Infrastructure.ServiceConfigurations
{
    public static class CustomAuthenticationServiceConfiguration
    {
        public static IServiceCollection ConfigureCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = services.GetHashedKey(configuration);

            var appSettings = configuration.GetSection(AppSettings.Key).Get<AppSettings>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = appSettings.JwtIssuer,
                        ValidateAudience = false
                    };
                })
                .AddCookie(x =>
                {
                    x.LoginPath = new PathString("/admin/auth/login");
                    // x.LogoutPath = new PathString("/admin/auth/logout");
                });

            return services;
        }
    }
}