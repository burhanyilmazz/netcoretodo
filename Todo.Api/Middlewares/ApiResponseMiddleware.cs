using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Todo.Core.Entities;
using Todo.Core.Enums;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Todo.Api.Middlewares
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<Startup> _localizer;
        public ApiResponseMiddleware(RequestDelegate next, IStringLocalizer<Startup> localizer)
        {
            _next = next;
            _localizer = localizer;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsSwagger(context))
                await _next(context);
            else
            {
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    context.Response.ContentType = "application/json";
                    try
                    {
                        await _next.Invoke(context);
                        var asd = context.Response;
                        var bodyString = await FormatResponse(context.Response);

                        if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            await HandleSuccessRequestAsync(context, bodyString);
                        }
                        else
                        {
                            await HandleNotSuccessRequestAsync(context, bodyString);
                        }
                    }
                    catch (Exception ex)
                    {
                        await HandleExceptionAsync(context, ex);
                    }
                    finally
                    {
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                    }
                }
            }

        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var message = exception.GetBaseException().Message;
            string stack = exception.StackTrace;

            var serviceResult = new ServiceResult
            {
                HttpStatusCode = HttpStatusCode.InternalServerError,
                HttpStatusText = HttpStatusCode.InternalServerError.ToString(),
                Message = message,
                MessageType = EMessageType.Error,
                MessageTypeText = EMessageType.Error.ToString(),
                Result = exception,
            };

            var json = JsonConvert.SerializeObject(serviceResult);
            return context.Response.WriteAsync(json);
        }

        private async Task HandleNotSuccessRequestAsync(HttpContext context, string bodyString)
        {
            var serviceResult = new ServiceResult
            {
                HttpStatusCode = (HttpStatusCode)context.Response.StatusCode,
                HttpStatusText = ((HttpStatusCode)context.Response.StatusCode).ToString()
            };

            switch (serviceResult.HttpStatusCode)
            {
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest:
                    serviceResult.Message = _localizer["Unauthorized"];
                    break;
                case HttpStatusCode.Forbidden:
                    serviceResult.Message = _localizer["Forbidden"];
                    break;
            }

            serviceResult.MessageType = EMessageType.Error;
            serviceResult.MessageTypeText = EMessageType.Error.ToString();
            serviceResult.Result = await Task.Run(() => JsonConvert.DeserializeObject<dynamic>(bodyString));

            var jsonString = JsonConvert.SerializeObject(serviceResult);
            await context.Response.WriteAsync(jsonString);
        }

        private async Task HandleSuccessRequestAsync(HttpContext context, string bodyString)
        {
            var serviceResult = new ServiceResult();

            var requestObject = await Task.Run(() => JsonConvert.DeserializeObject<ServiceResult>(bodyString));
            serviceResult.HttpStatusCode = requestObject.HttpStatusCode == 0 ? (HttpStatusCode)context.Response.StatusCode : requestObject.HttpStatusCode;
            serviceResult.HttpStatusText = requestObject.HttpStatusCode == 0 ? ((HttpStatusCode)context.Response.StatusCode).ToString() : requestObject.HttpStatusText;
            serviceResult.Message = requestObject.Message;
            serviceResult.MessageType = requestObject.MessageType;
            serviceResult.MessageTypeText = requestObject.MessageType.ToString();
            serviceResult.Result = requestObject.Result;
            serviceResult.ShowNotification = requestObject.ShowNotification;
            serviceResult.Title = requestObject.Title;
            
            var jsonString = JsonConvert.SerializeObject(serviceResult);
            await context.Response.WriteAsync(jsonString);
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return plainBodyText;
        }

        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }
    }
}
