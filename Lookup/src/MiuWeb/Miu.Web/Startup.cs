using Demo.Web.Models;
using Demo.Web.Services;
using Demo.Web.Views;
using GoC.WebTemplate.Components.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Demo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }
        public ILogger<Startup> Logger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddModelAccessor();
            services.AddHttpContextAccessor();
            services.AddTransient<LayoutViewModel>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/Pages");
            services.AddDistributedMemoryCache();
            services.AddHttpClient();

            services.AddScoped<IInteropService, InteropService>();
            services.Configure<AddressApi>(Configuration.GetSection("AddressApi"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            //    await next();
            //});
            Logger.LogTrace(env.EnvironmentName);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UsePathBase(Configuration["BaseUrl"]);
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-CA"),
                new CultureInfo("fr-CA"),
            };

            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture((supportedCultures[0]).Name),                
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            requestLocalizationOptions.RequestCultureProviders.Clear();

            app.UseRequestLocalization(requestLocalizationOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }


    }
}
