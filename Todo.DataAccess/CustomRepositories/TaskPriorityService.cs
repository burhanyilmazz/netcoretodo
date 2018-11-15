using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.DataAccess.Infrastructure;
using Todo.Domain;
using Dapper.FastCrud;

namespace Todo.DataAccess.CustomRepositories
{
    public class TaskPriorityService : IGenericRepository<TaskPriority>, ITaskPriorityRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public TaskPriorityService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public TaskPriority Add(TaskPriority entity)
        {
            throw new NotImplementedException();
        }

        public Task<TaskPriority> AddAsync(TaskPriority entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(TaskPriority entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TaskPriority entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskPriority> Filter()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskPriority>> FilterAsync()
        {
            throw new NotImplementedException();
        }

        public TaskPriority Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskPriority> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TaskPriority>> GetAllAsync()
        {
            return await _connectionFactory.GetConnection.FindAsync<TaskPriority>();
        }

        public Task<TaskPriority> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(TaskPriority entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TaskPriority entity)
        {
            throw new NotImplementedException();
        }
    }
}

