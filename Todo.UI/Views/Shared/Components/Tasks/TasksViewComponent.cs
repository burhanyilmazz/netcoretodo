using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Todo.Core.Entities;
using Todo.Domain.ViewModels;

namespace ModelCommunity.Web.Views.Shared.Components.HeaderMain
{
    public class TasksViewComponent : ViewComponent
    {
        private readonly IOptions<ServiceEndpoint> _serviceEndpoint;
        private readonly IOptions<ProjectInfo> _projectInfo;

        public TasksViewComponent(IOptions<ServiceEndpoint> serviceEndpoint, IOptions<ProjectInfo> projectInfo)
        {
            _serviceEndpoint = serviceEndpoint;
            _projectInfo = projectInfo;
        }

        public async Task<IViewComponentResult> InvokeAsync(int statusId)
        {
            var signedUserCookie = HttpContext.Request.Cookies["AccessToken"];
            var token = JsonConvert.DeserializeObject<TokenResponse>(signedUserCookie);

            var serviceResult = await _serviceEndpoint.Value.HostName
                                 .AppendPathSegment(_serviceEndpoint.Value.TaskEndpoint.GetTaskViewModel)
                                 .WithHeader("Authorization", "Bearer " + token.AccessToken)
                                 .PostJsonAsync(statusId)
                                 .ReceiveJson<ServiceResult>();

            if (serviceResult.MessageType == Todo.Core.Enums.EMessageType.Success)
            {
                serviceResult.Result = JsonConvert.DeserializeObject<TaskViewModel>(serviceResult.Result.ToString());
                serviceResult.Title = statusId.ToString();
            }
            return View(serviceResult);
        }

    }
}
