using BioMad_backend.Helpers;
using BioMad_backend.Infrastructure.AbstractClasses;
using BioMad_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddScoped<SocialAuthenticationService, SocialAuthenticationServices.GoogleAuthenticationService>();
            services.AddScoped<SocialAuthenticationService, SocialAuthenticationServices.VkAuthenticationService>();
            services.AddScoped<SocialAuthenticationService, SocialAuthenticationServices.FacebookAuthenticationService>();

            return services;
        }
    }
}