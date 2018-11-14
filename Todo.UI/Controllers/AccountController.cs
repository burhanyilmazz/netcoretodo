using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Todo.Core.Entities;
using Todo.Core.ViewModels.Membership;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace Todo.UI.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IOptions<ServiceEndpoint> _serviceEndpoint;
        public AccountController(IOptions<ServiceEndpoint> serviceEndpoint)
        {
            _serviceEndpoint = serviceEndpoint;
        }
        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (HttpContext.User.Identity != null && !string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserSignInWithFormVM user)
        {
            var loginResult = await _serviceEndpoint.Value.HostName
               .AppendPathSegment(_serviceEndpoint.Value.AccountEndpoint.SignInWithForm)
               .PostJsonAsync(user)
               .ReceiveJson<ServiceResult>();

            if (loginResult.MessageType != Core.Enums.EMessageType.Success)
            {
                ViewData["Result"] = loginResult;
                return View();
            }

            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Append("AccessToken", loginResult.Result.ToString());

            return RedirectToAction("Index", "Home");
        }
    }
}