using BioMad_backend.Helpers;
using BioMad_backend.Infrastructure.ServiceConfigurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SocialAuthenticationCore.Extensions;

namespace BioMad_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("config.json").AddConfiguration(configuration);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = services.ConfigureAppSettings(Configuration);

            services.ConfigureDbService(Configuration)
                .ConfigureSwaggerService()
                .AddHelperServices()
                .AddSocialAuthenticationService(options => options.GoogleClientId = appSettings.GoogleClientId)
                .ConfigureCustomAuthentication(Configuration)
                .ConfigureLocalization()
                .AddLocalization(opts => opts.ResourcesPath = "Resources")
                .AddCors()
                .AddControllersWithViews(options => options.Conventions.Add(new ControllerVersioningConvention()))
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseStaticFiles();
            app.UseSwaggerWithCustomConfiguration();


            app.UseLocalization();
            
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "Admin",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "Share",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}