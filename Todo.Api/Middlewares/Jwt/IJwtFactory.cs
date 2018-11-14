using Todo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Domain;

namespace Todo.Api.Middlewares.Jwt
{
    public interface IJwtFactory
    {
        Task<TokenResponse> GenerateEncodedToken(User user, UserRefreshToken userRefreshToken);
        Task<ClaimsIdentity> GenerateClaimsIdentity(User user);
    }
}
