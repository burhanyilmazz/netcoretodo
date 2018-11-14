using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace Todo.Api.Middlewares.Jwt
{
    public class JwtAuthenticationOptions
    {
        public string Issuer { get; set; }

        public string Subject { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime Expiration => IssuedAt.Add(ValidFor);

        public DateTime NotBefore => DateTime.UtcNow;

        public DateTime IssuedAt => DateTime.UtcNow;

        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(1);

        public TimeSpan RefreshTokenValidFor { get; set; } = TimeSpan.FromDays(1);

        public DateTime RefreshTokenExpiration => IssuedAt.Add(RefreshTokenValidFor);

        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());

        public SigningCredentials SigningCredentials { get; set; }
    }
}
