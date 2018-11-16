using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Todo.Api.Middlewares;
using Todo.Core.Entities;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Reflection;
using Todo.Api;

namespace ModelCommunity.Api.Extensions
{
    public static class SwaggerAndVersioningExtension
    {   
        public static void ConfigureSwaggerAndVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvcCore().AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";                    
                    options.SubstituteApiVersionInUrl = true;
                });
            

            
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });

            services.AddSwaggerGen(
                options =>
                {
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, configuration));
                    }
                    options.OperationFilter<SwaggerDefaultValues>();
                    //options.IncludeXmlComments(XmlCommentsFilePath);
                });
        }

        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        private static Info CreateInfoForApiVersion(ApiVersionDescription description, IConfiguration configuration)
        {
            var projectInfo = configuration.GetSection("ProjectInfo").Get<ProjectInfo>();

            var info = new Info()
            {
                Title = projectInfo.Name + " " + description.ApiVersion,
                Version = description.ApiVersion.ToString(),
                Description = projectInfo.Description,
                Contact = new Swashbuckle.AspNetCore.Swagger.Contact { Name = projectInfo.Contact.Name, Email = projectInfo.Contact.Email, Url = projectInfo.Contact.Web }
            };

            if (description.IsDeprecated)
            {
                info.Description += projectInfo.Api.DeprecatedText;
            }

            return info;
        }
    }
}
