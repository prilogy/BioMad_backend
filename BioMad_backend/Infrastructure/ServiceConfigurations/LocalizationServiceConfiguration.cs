using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BioMad_backend.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BioMad_backend.Infrastructure.ServiceConfigurations
{
    public static class LocalizationServiceConfiguration
    {
        private static readonly List<CultureInfo> SupportedCultures = Culture.All.Select(x => x.Info).ToList();

        private static readonly RequestCulture DefaultRequestCulture =
            new RequestCulture(culture: Culture.En.Key, uiCulture: Culture.En.Key);
        
        
        public static IServiceCollection ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(opts => opts.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    opts.DefaultRequestCulture = DefaultRequestCulture;
                    opts.SupportedCultures = SupportedCultures;
                    opts.SupportedUICultures = SupportedCultures;
                    opts.RequestCultureProviders = new[]
                    {
                        new RouteDataRequestCultureProvider
                        {
                            Options =  opts,
                            RouteDataStringKey = "culture",
                            UIRouteDataStringKey = "culture"
                        }
                    };
                });

            return services;
        }

        public static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = DefaultRequestCulture,
                SupportedCultures = SupportedCultures,
                SupportedUICultures = SupportedCultures
            });

            return app;
        }
    }
}