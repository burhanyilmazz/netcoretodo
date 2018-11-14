using Microsoft.Extensions.Options;
using Todo.Core.Entities;
using Todo.Domain;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Todo.Api.Middlewares.Jwt
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtAuthenticationOptions _jwtOptions;

        public JwtFactory(IOptions<JwtAuthenticationOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<TokenResponse> GenerateEncodedToken(User user, UserRefreshToken refreshToken)
        {
            var claimsIdentity = await GenerateClaimsIdentity(user);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claimsIdentity.Claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var response = new TokenResponse
            {
                Id = claimsIdentity.Claims.Single(c => c.Type == "id").Value,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpiresIn = (int)_jwtOptions.ValidFor.TotalSeconds,
                RefreshToken = refreshToken.Token.ToString(),
                TokenExpirationUtc = jwt.ValidTo
            };

            return response;
        }

        public async Task<ClaimsIdentity> GenerateClaimsIdentity(User user)
        {
            var claimsIdentity = await Task.Run(() => new ClaimsIdentity(new GenericIdentity(user.Username, "Token"), new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("userName", string.IsNullOrEmpty(user.Username) ? string.Empty : user.Username)
            }));
           
            return claimsIdentity;
        }
        
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtAuthenticationOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));


            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtAuthenticationOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtAuthenticationOptions.JtiGenerator));
            }
        }


    }
}
