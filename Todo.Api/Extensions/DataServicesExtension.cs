using Microsoft.Extensions.DependencyInjection;
using Todo.DataAccess.CustomRepositories;
using Todo.DataAccess.Infrastructure;

namespace Todo.Api.Middlewares
{
    public static class DataServicesExtension
    {
        public static void ConfigureDataServices(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IConnectionFactory>(s => new ConnectionFactory(connectionString));
            services.AddTransient<IUserRepository, UserService>();
            services.AddTransient<ITaskPriorityRepository, TaskPriorityService>();
            services.AddTransient<ITaskStatusRepository, TaskStatusService>();
            services.AddTransient<ITaskRepository, TaskService>();
        }
    }
}
