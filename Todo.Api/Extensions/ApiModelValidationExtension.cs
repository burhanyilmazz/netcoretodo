using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Todo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Todo.Api.Extensions
{
    public static class ApiModelValidationExtension
    {
        public static void ConfigureApiModelState(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var serviceResult = new ServiceResult
                    {
                        HttpStatusCode = HttpStatusCode.BadRequest,
                        HttpStatusText = (HttpStatusCode.BadRequest).ToString(),
                        MessageType = Core.Enums.EMessageType.Error,
                        MessageTypeText = Core.Enums.EMessageType.Error.ToString(),
                        ShowNotification = true,
                        Message = "ModelStateError",
                        Result = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new FaultyModel
                        {
                            PropertyName = e.Key,
                            Errors = e.Value.Errors.Select(y => y.ErrorMessage).ToList()
                        }).ToList()
                    };


                    return new BadRequestObjectResult(serviceResult);
                };
                options.SuppressModelStateInvalidFilter = true;
            });

        }

        
    }
}
