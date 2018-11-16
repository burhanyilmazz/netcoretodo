using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModelCommunity.Api.Controllers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Api.ActionFilters;
using Todo.Api.Middlewares.Jwt;
using Todo.Core.Entities;
using Todo.DataAccess.CustomRepositories;
using Todo.Domain;
using Todo.Domain.ViewModels;
using Todo.UI.ViewModels;

namespace ModelCommunity.Api.V1.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    [ApiController]
    public class TaskController : BaseController
    {
        private readonly IUserRepository _userService;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtAuthenticationOptions _jwtOptions;
        private readonly ClaimsPrincipal _caller;
        private readonly IConfiguration _configuration;
        private readonly ITaskRepository _taskService;
        private readonly ITaskPriorityRepository _taskPriorityService;
        private readonly ITaskStatusRepository _taskStatusService;


        public TaskController(IUserRepository userService, ITaskRepository taskService, ITaskPriorityRepository taskPriorityService, ITaskStatusRepository taskStatusService, IJwtFactory jwtFactory, IOptions<JwtAuthenticationOptions> jwtOptions, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(configuration)
        {
            _userService = userService;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _caller = httpContextAccessor.HttpContext.User;
            _configuration = configuration;
            _taskService = taskService;
            _taskPriorityService = taskPriorityService;
            _taskStatusService = taskStatusService;
        }

        [HttpPost("GetPriorities")]
        [ApiAuthorize]
        public async Task<ServiceResult> GetPriorities()
        {           
            var taskPriorities = await _taskPriorityService.GetAllAsync();           

            ServiceResult = new ServiceResult
            {
                MessageType = Todo.Core.Enums.EMessageType.Success,
                Result = taskPriorities.ToList()
            };

            return ServiceResult;
        }

        [HttpPost("GetTaskViewModel")]
        [ApiAuthorize]
        public async Task<ServiceResult> GetTaskViewModel([FromBody] int statusId)
        {
            var userId = int.Parse(_caller.Claims.FirstOrDefault(x => x.Type == "id").Value);

            var taskPriorities = await _taskPriorityService.GetAllAsync();
            var taskStatuses = await _taskStatusService.GetAllAsync();
            var tasks = await _taskService.GetTasks(userId);
            foreach (var item in taskStatuses)
            {
                item.TaskCount = tasks.Count(x => x.TaskStatusId == item.Id);
            }

            ServiceResult = new ServiceResult
            {
                MessageType = Todo.Core.Enums.EMessageType.Success,
                Result = new TaskViewModel
                {
                    TaskPriorities = taskPriorities.ToList(),
                    TaskStatuses = taskStatuses.ToList(),
                    Tasks = tasks.Where(x=>x.TaskStatusId == statusId || statusId == 0).ToList()
                }
            };

            return ServiceResult;
        }

        [HttpPost("AddTask")]
        [ApiAuthorize]
        public async Task<ServiceResult> AddTask([FromBody] AddTaskViewModel addTaskModel)
        {
            var userId = int.Parse(_caller.Claims.FirstOrDefault(x => x.Type == "id").Value);

            var todoTask = await _taskService.AddAsync(new TodoTask
            {
                CreatedUserId = userId,
                Name = addTaskModel.Name,
                TaskPriorityId = addTaskModel.TaskPriorityId.Value,
                TaskStatusId = 1,
            });

            ServiceResult = new ServiceResult
            {
                MessageType = Todo.Core.Enums.EMessageType.Success,
                Message = "Task added.",
                Result = todoTask
            };

            return ServiceResult;
        }

        [HttpPost("UpdateTask")]
        [ApiAuthorize]
        public async Task<ServiceResult> UpdateTask([FromBody] AddTaskViewModel updateTaskModel)
        {
            var todoTask = await _taskService.GetAsync(updateTaskModel.TaskId.Value);

            todoTask.LastModifiedDate = DateTime.Now;

            if (!string.IsNullOrEmpty(updateTaskModel.Name))
                todoTask.Name = updateTaskModel.Name;

            if (updateTaskModel.TaskStatusId.HasValue)
                todoTask.TaskStatusId = updateTaskModel.TaskStatusId.Value;

            var status = await _taskService.UpdateAsync(todoTask);

            ServiceResult = new ServiceResult
            {
                MessageType = status ? Todo.Core.Enums.EMessageType.Success : Todo.Core.Enums.EMessageType.Error,
                Message = status ? "Task updated." : "Something went wrong.",
                Result = todoTask
            };
            return ServiceResult;
        }

       
    }
}