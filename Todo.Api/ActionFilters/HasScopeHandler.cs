using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Todo.Api.Middlewares.Jwt;
using System;
using System.Threading.Tasks;

namespace Todo.Api.ActionFilters
{
    public class ApiAuthorize : AuthorizeAttribute
    {
        public ApiAuthorize() { }

    }

    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }
        
        public HasScopeRequirement(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }

    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "rights" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;
            
            if (context.User.HasClaim(s => s.Type == "rights" && s.Value == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;
        private readonly IConfiguration _configuration;
        private readonly JwtAuthenticationOptions _jwtOptions;

        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IConfiguration configuration, IOptions<JwtAuthenticationOptions> jwtOptions) : base(options)
        {
            _options = options.Value;
            _configuration = configuration;
            _jwtOptions = jwtOptions.Value;
        }
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy == null)
            {
                policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new HasScopeRequirement(policyName, _jwtOptions.Issuer))
                    .Build();
                
                _options.AddPolicy(policyName, policy);
            }

            return policy;
        }
    }
}
