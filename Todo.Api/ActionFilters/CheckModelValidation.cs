using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Todo.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Todo.Api.ActionFilters
{
    public class CheckModelValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var localize = context.HttpContext.RequestServices.GetService<IStringLocalizer<Startup>>();
                var serviceResult = new ServiceResult
                {
                    HttpStatusCode = (HttpStatusCode)context.HttpContext.Response.StatusCode,
                    HttpStatusText = ((HttpStatusCode)context.HttpContext.Response.StatusCode).ToString(),
                    MessageType = Core.Enums.EMessageType.Error,
                    Result = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new FaultyModel
                        {
                            PropertyName = e.Key,
                            Errors = e.Value.Errors.Select(y => y.ErrorMessage).ToList()
                        }).ToList(),
                    Message = "Model"
                };
                foreach (FaultyModel item in (List<FaultyModel>)serviceResult.Result)   {
                    for (int i = 0; i < item.Errors.Count; i++) {
                        item.Errors[i] = string.Format(localize[item.Errors[i]], item.PropertyName);
                    }
                }

                context.Result = new BadRequestObjectResult(serviceResult);
            }
        }
    }
}
