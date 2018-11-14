using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Todo.Core.Entities;

namespace ModelCommunity.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IConfiguration Configuration;
        public ProjectInfo ProjectInfo;

        public BaseController(IConfiguration configuration)
        {
            Configuration = configuration;
            ProjectInfo = configuration.GetSection("ProjectInfo").Get<ProjectInfo>();
        }

        public ServiceResult ServiceResult { get; set; }
    }
}