using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModelCommunity.Api.Controllers;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Api.ActionFilters;
using Todo.Api.Middlewares.Jwt;
using Todo.Core.Entities;
using Todo.Core.ViewModels.Membership;
using Todo.DataAccess.CustomRepositories;
using Todo.Domain;

namespace ModelCommunity.Api.V1.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IUserRepository _userService;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtAuthenticationOptions _jwtOptions;
        private readonly ClaimsPrincipal _caller;
        private readonly IConfiguration _configuration;


        public AuthController(IUserRepository userService, IJwtFactory jwtFactory, IOptions<JwtAuthenticationOptions> jwtOptions, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(configuration)
        {
            _userService = userService;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _caller = httpContextAccessor.HttpContext.User;
            _configuration = configuration;
        }

        [HttpPost("SignInWithForm")]
        [CheckModelValidation]
        [AllowAnonymous]
        public async Task<ServiceResult> SignInWithForm([FromBody] UserSignInWithFormVM userSignInWithFormVM)
        {
            var serviceResult = await _userService.SignInWithFormAsync(userSignInWithFormVM);

            if (serviceResult.MessageType != Todo.Core.Enums.EMessageType.Success)
            {
                return serviceResult;
            }

            var user = (User)serviceResult.Result;

            _userService.RemoveOldRefreshTokensAsync(user.Id);

            var newRefreshToken = new UserRefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                IssuedUtc = _jwtOptions.IssuedAt,
                ExpiresUtc = _jwtOptions.RefreshTokenExpiration
            };

            await _userService.AddRefreshTokenAsync(newRefreshToken);

            var jwt = await _jwtFactory.GenerateEncodedToken(user, newRefreshToken);
            serviceResult = new ServiceResult { Result = jwt, MessageType = Todo.Core.Enums.EMessageType.Success };

            return serviceResult;
        }

        [AllowAnonymous]
        [CheckModelValidation]
        [HttpPost("RefreshToken")]
        public async Task<ServiceResult> RefreshToken([FromBody] UserRefreshTokenVM refreshToken)
        {
            var serviceResult = new ServiceResult();
            var refreshTokenFromDatabase = await _userService.GetUserRefreshTokensAsync(refreshToken);

            if (refreshTokenFromDatabase == null)
            {
                serviceResult.HttpStatusCode = HttpStatusCode.BadRequest;
                serviceResult.HttpStatusText = HttpStatusCode.BadRequest.ToString();
                serviceResult.Message = "Unauthorized";
                serviceResult.MessageType = Todo.Core.Enums.EMessageType.Error;
                serviceResult.MessageTypeText = serviceResult.MessageType.ToString();
                return serviceResult;
            }

            if (refreshTokenFromDatabase.ExpiresUtc < DateTime.UtcNow)
            {
                serviceResult.HttpStatusCode = HttpStatusCode.Unauthorized;
                serviceResult.HttpStatusText = HttpStatusCode.Unauthorized.ToString();
                serviceResult.Message = "Unauthorized";
                serviceResult.MessageType = Todo.Core.Enums.EMessageType.Error;
                serviceResult.MessageTypeText = serviceResult.MessageType.ToString();
                return serviceResult;
            }
            
            var jwt = await _jwtFactory.GenerateEncodedToken(refreshTokenFromDatabase.User, refreshTokenFromDatabase);
            serviceResult = new ServiceResult { Result = jwt, MessageType = Todo.Core.Enums.EMessageType.Success };

            return serviceResult;
        }

        [HttpPost("GetSignedUserData")]
        [ApiAuthorize]

        public async Task<ServiceResult> GetSignedUserData()
        {
            var signedData = new SignedUserData()
            {

                Id = int.Parse(_caller.Claims.FirstOrDefault(x => x.Type == "id").Value),
                UserName = _caller.Claims.FirstOrDefault(x => x.Type == "userName").Value
            };
           
            ServiceResult = new ServiceResult
            {
                MessageType = Todo.Core.Enums.EMessageType.Success,
                Result = signedData
            };

            return await Task.Run(() => ServiceResult);
        }

    }
}