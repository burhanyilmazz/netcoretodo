using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Todo.UI.ActionFilters
{
    public class AuthenticationFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity == null || string.IsNullOrEmpty(context.HttpContext.User.Identity.Name))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {{ "Controller", "Account" }, { "Action", "Login" } });
                return;
            }
            var resultContext = await next();
        }
    }
}
