using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Todo.Api.ActionFilters;
using Todo.Api.Middlewares.Jwt;

namespace Todo.Api.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<CheckModelValidation>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        }
    }
}
