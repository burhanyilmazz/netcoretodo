using Todo.DataAccess.Infrastructure;
using Todo.Domain;

namespace Todo.DataAccess.CustomRepositories
{
    public interface ITaskPriorityRepository : IGenericRepository<TaskPriority>
    {
    }
}
