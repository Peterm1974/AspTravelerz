using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AspTravelerz
{
    public class Startup
    {
        public string HomeTimeZone { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            loggerFactory.AddConsole(LogLevel.Information);
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> { "index.html", "defaultFile.html" }
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "assets")),
                RequestPath = new PathString("/assets")
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "assets")),
                RequestPath = new PathString("/assets")
            });

            app.UseMvc(routes =>
                routes.MapRoute(
                  name: "default",
                  template: "{controller = Home}/{action = Index}/{id ?}"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var title = configuration["AppSettings:SiteTitle"];
            var secret = configuration["AppSettings_AnalyticsId"];

            app.Run(async (context) =>
                        {
                            await context.Response.WriteAsync($"Hello World! The site is called {title}, the secret value is {secret}");
                        });
        }
    }
}
