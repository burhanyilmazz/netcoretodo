using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Todo.Core.Entities;
using System;
using System.IO;
using ModelCommunity.Web.Middlewares;

namespace ModelCommunity.Web
{
    public class Startup
    {
        public IHostingEnvironment HostingEnvironment;
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env, IServiceProvider serviceProvider)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            HostingEnvironment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {      
            services.AddOptions();

            var serviceEndpointSection = Configuration.GetSection("ServiceEndpoint");
            services.Configure<ServiceEndpoint>(serviceEndpointSection);

            var projectInfoSection = Configuration.GetSection("ProjectInfo");
            services.Configure<ProjectInfo>(projectInfoSection);
            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            });

            app.UseCookiePolicy();

            app.UseMiddleware<SignedUserMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
