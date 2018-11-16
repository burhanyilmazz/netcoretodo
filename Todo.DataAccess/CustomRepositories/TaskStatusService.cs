using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.DataAccess.Infrastructure;
using TaskStatus = Todo.Domain.TaskStatus;
using Dapper.FastCrud;

namespace Todo.DataAccess.CustomRepositories
{
    public class TaskStatusService : IGenericRepository<TaskStatus>, ITaskStatusRepository
    {
        private IConnectionFactory _connectionFactory;

        public TaskStatusService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public TaskStatus Add(TaskStatus entity)
        {
            throw new NotImplementedException();
        }

        public Task<TaskStatus> AddAsync(TaskStatus entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(TaskStatus entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TaskStatus entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskStatus> Filter()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskStatus>> FilterAsync()
        {
            throw new NotImplementedException();
        }

        public TaskStatus Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskStatus> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TaskStatus>> GetAllAsync()
        {
            return await _connectionFactory.GetConnection.FindAsync<TaskStatus>(x => x.OrderBy($"{nameof(TaskStatus.Id):C}"));
        }

        public Task<TaskStatus> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(TaskStatus entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TaskStatus entity)
        {
            throw new NotImplementedException();
        }
    }
}

