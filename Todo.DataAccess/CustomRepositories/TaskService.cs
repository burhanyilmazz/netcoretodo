using Dapper.FastCrud;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.DataAccess.Infrastructure;
using Todo.Domain;

namespace Todo.DataAccess.CustomRepositories
{
    public class TaskService : IGenericRepository<TodoTask>, ITaskRepository
    {
        private IConnectionFactory _connectionFactory;

        public TaskService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public TodoTask Add(TodoTask entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TodoTask> AddAsync(TodoTask entity)
        {
            await _connectionFactory.GetConnection.InsertAsync(entity);
            return entity;
        }

        public bool Delete(TodoTask entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TodoTask entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TodoTask> Filter()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TodoTask>> FilterAsync()
        {
            throw new NotImplementedException();
        }

        public TodoTask Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TodoTask> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TodoTask>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TodoTask> GetAsync(int id)
        {
            return await _connectionFactory.GetConnection.GetAsync(new TodoTask { Id = id });
        }

        public async Task<IEnumerable<TodoTask>> GetTasks(int userId)
        {
            return await _connectionFactory.GetConnection.FindAsync<TodoTask>(x => x
                              .Where($"{nameof(TodoTask.CreatedUserId):C} = @userId")
                              .OrderBy($"{nameof(TodoTask.CreatedDate):C} DESC")
                              .WithParameters(new { userId }));

        }

        public async Task<int> GetUserTaskCount(int userId, int taskStatusId)
        {
            return await _connectionFactory.GetConnection.CountAsync<TodoTask>(x => x
               .Where($"{nameof(TodoTask.CreatedUserId):C} = @userId")
               .Where($"{nameof(TodoTask.TaskStatusId):C} = @taskStatusId")
               .WithParameters(new { userId, taskStatusId }));
        }

        public bool Update(TodoTask entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(TodoTask entity)
        {
            return await _connectionFactory.GetConnection.UpdateAsync(entity);
        }
    }
}

