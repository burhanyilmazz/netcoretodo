using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.DataAccess.Infrastructure;
using TaskStatus = Todo.Domain.TaskStatus;

namespace Todo.DataAccess.CustomRepositories
{
    public interface ITaskStatusRepository : IGenericRepository<TaskStatus>
    {
    }
}
