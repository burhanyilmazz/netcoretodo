using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Entities;
using Todo.Domain;
using Todo.Domain.ViewModels;
using Todo.UI.ActionFilters;
using Todo.UI.ViewModels;

namespace Todo.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<ServiceEndpoint> _serviceEndpoint;
        public HomeController(IOptions<ServiceEndpoint> serviceEndpoint)
        {
            _serviceEndpoint = serviceEndpoint;
        }

        [AuthenticationFilter]
        public async Task<IActionResult> Index()
        {
            var signedUserCookie = HttpContext.Request.Cookies["AccessToken"];
            var token = JsonConvert.DeserializeObject<TokenResponse>(signedUserCookie);
            var serviceResult = await _serviceEndpoint.Value.HostName
                                 .AppendPathSegment(_serviceEndpoint.Value.TaskEndpoint.GetPriorities)
                                 .WithHeader("Authorization", "Bearer " + token.AccessToken)
                                 .PostJsonAsync(new { })
                                 .ReceiveJson<ServiceResult>();

            if (serviceResult.MessageType == Core.Enums.EMessageType.Success)
            {
                serviceResult.Result = JsonConvert.DeserializeObject<List<TaskPriority>>(serviceResult.Result.ToString());
            }
            return View(serviceResult);
        }

        [AuthenticationFilter]
        public async Task<ServiceResult> AddTask([FromBody] AddTaskViewModel taskModel)
        {
            var signedUserCookie = HttpContext.Request.Cookies["AccessToken"];
            var token = JsonConvert.DeserializeObject<TokenResponse>(signedUserCookie);

            var serviceResult = await _serviceEndpoint.Value.HostName
                                 .AppendPathSegment(_serviceEndpoint.Value.TaskEndpoint.AddTask)
                                 .WithHeader("Authorization", "Bearer " + token.AccessToken)
                                 .PostJsonAsync(taskModel)
                                 .ReceiveJson<ServiceResult>();

            if (serviceResult.MessageType == Core.Enums.EMessageType.Success)
            {
                serviceResult.Result = JsonConvert.DeserializeObject<TodoTask>(serviceResult.Result.ToString());
            }
            return serviceResult;
        }

        [AuthenticationFilter]
        public async Task<ServiceResult> UpdateTask([FromBody] AddTaskViewModel taskModel)
        {
            var signedUserCookie = HttpContext.Request.Cookies["AccessToken"];
            var token = JsonConvert.DeserializeObject<TokenResponse>(signedUserCookie);

            var serviceResult = await _serviceEndpoint.Value.HostName
                                 .AppendPathSegment(_serviceEndpoint.Value.TaskEndpoint.UpdateTask)
                                 .WithHeader("Authorization", "Bearer " + token.AccessToken)
                                 .PostJsonAsync(taskModel)
                                 .ReceiveJson<ServiceResult>();

            if (serviceResult.MessageType == Core.Enums.EMessageType.Success)
            {
                serviceResult.Result = JsonConvert.DeserializeObject<TodoTask>(serviceResult.Result.ToString());
            }
            return serviceResult;
        }

        [HttpGet]
        [AuthenticationFilter]
        public async Task<IActionResult> GetTasks(int statusId)
        {
            var signedUserCookie = HttpContext.Request.Cookies["AccessToken"];
            var token = JsonConvert.DeserializeObject<TokenResponse>(signedUserCookie);

            var serviceResult = await _serviceEndpoint.Value.HostName
                                 .AppendPathSegment(_serviceEndpoint.Value.TaskEndpoint.GetTaskViewModel)
                                 .WithHeader("Authorization", "Bearer " + token.AccessToken)
                                 .PostJsonAsync(new { })
                                 .ReceiveJson<ServiceResult>();

            if (serviceResult.MessageType == Core.Enums.EMessageType.Success)
            {
                serviceResult.Result = JsonConvert.DeserializeObject<TaskViewModel>(serviceResult.Result.ToString());
            }

            return ViewComponent("Tasks", statusId);

        }


    }
}
