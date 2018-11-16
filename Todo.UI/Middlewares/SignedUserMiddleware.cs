using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Todo.Core.Entities;
using Todo.Domain.ViewModels;

namespace ModelCommunity.Web.Middlewares
{
    public class SignedUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<ServiceEndpoint> _serviceEndpoint;

        public SignedUserMiddleware(RequestDelegate next, IOptions<ServiceEndpoint> serviceEndpoint)
        {
            _next = next;
            _serviceEndpoint = serviceEndpoint;
        }

        public GenericPrincipal BuildSignedUserData(ServiceResult serviceResult)
        {
            var signedUserData = JsonConvert.DeserializeObject<SignedUserData>(serviceResult.Result.ToString());

            var claimsIdentity = new ClaimsIdentity(new GenericIdentity(signedUserData.UserName, "Token"), new[]
                      {
                        new Claim("id", signedUserData.Id.ToString()),
                        new Claim("userName", string.IsNullOrEmpty(signedUserData.UserName) ? string.Empty : signedUserData.UserName),
                    });

            var signedUser = new GenericPrincipal(claimsIdentity, new string[] { });
            return signedUser;
        }

        public async Task<ServiceResult> GetSignedUserData(TokenResponse token)
        {
            var serviceResult = await _serviceEndpoint.Value.HostName
                    .AppendPathSegment(_serviceEndpoint.Value.AccountEndpoint.SignedUserDataEndpoint)
                    .WithHeader("Authorization", "Bearer " + token.AccessToken)
                    .PostJsonAsync(new { })
                    .ReceiveJson<ServiceResult>();

            return serviceResult;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var signedUserCookie = context.Request.Cookies["AccessToken"];

            if (signedUserCookie == null)
            {
                context.User = null;
            }
            else
            {
                var token = JsonConvert.DeserializeObject<TokenResponse>(signedUserCookie);

                try
                {
                    var signedUserResult = await GetSignedUserData(token);

                    context.User = BuildSignedUserData(signedUserResult);
                }
                catch (FlurlHttpException ex)
                {
                    var error = await ex.GetResponseJsonAsync<ServiceResult>();
                    if (error.HttpStatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        context.Response.Cookies.Delete("AccessToken");

                        var refreshTokenResult = await _serviceEndpoint.Value.HostName
                            .AppendPathSegment(_serviceEndpoint.Value.AccountEndpoint.RefreshToken)
                            .PostJsonAsync(new RefreshTokenViewModel
                            {
                                Token = token.RefreshToken
                            })
                            .ReceiveJson<ServiceResult>();

                        if (refreshTokenResult.MessageType != Todo.Core.Enums.EMessageType.Success)
                        {
                            context.Response.Redirect("/Account/Login/");
                            return;
                        }

                        token = JsonConvert.DeserializeObject<TokenResponse>(refreshTokenResult.Result.ToString());

                        var signedUserResult = await GetSignedUserData(token);

                        if (signedUserResult.MessageType != Todo.Core.Enums.EMessageType.Success)
                        {
                            context.Response.Redirect("/Account/Login/");
                            return;
                        }

                        context.Response.Cookies.Append("AccessToken", refreshTokenResult.Result.ToString());
                        context.User = BuildSignedUserData(signedUserResult);
                    }
                }

            }
            await _next(context);
        }
    }
}
