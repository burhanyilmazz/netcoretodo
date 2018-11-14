using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelCommunity.Api.Extensions;
using System;
using Todo.Api.Extensions;
using Todo.Api.Middlewares;

namespace Todo.Api
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
            services.ConfigureServices(Configuration);

            services.ConfigureDataServices(Configuration.GetConnectionString("DefaultConnection"));

            services.AddOptions();

            services.ConfigureSwaggerAndVersioning(Configuration);

            services.ConfigureAuthentication(Configuration);

            services.ConfigureCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddDataAnnotationsLocalization();

            services.ConfigureApiModelState();
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseHsts();

            app.UseSwagger(provider);

            app.UseApiResponseWrapperMiddleware();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}
