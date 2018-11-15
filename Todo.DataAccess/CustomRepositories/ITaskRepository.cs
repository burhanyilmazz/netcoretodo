using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.DataAccess.Infrastructure;
using Todo.Domain;

namespace Todo.DataAccess.CustomRepositories
{
    public interface ITaskRepository : IGenericRepository<TodoTask>
    {
        Task<int> GetUserTaskCount(int userId, int taskStatusId);
        Task<IEnumerable<TodoTask>> GetTasks(int userId);
    }
}
