using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todo.DataAccess.Infrastructure
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        Task<TEntity> GetAsync(int id);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> Filter();
        Task<IEnumerable<TEntity>> FilterAsync();
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        bool Delete(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        bool Update(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
    }
}
