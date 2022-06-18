using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WemaBankTask.Entities.Model;

namespace WemaBankTask.Entities.IRepository
{
    public interface ICustomerRepository<TEntity> where TEntity : class
    {
        Task<bool> SaveAsync(TEntity entity);

        bool Save();

        bool Remove(TEntity entity);

        bool Remove(object id);

        bool AddRange(IEnumerable<TEntity> entity);

        Task<bool> AddRangeAsync(IEnumerable<TEntity> entity);

        bool Add(TEntity entity);

        Task<bool> AddAsync(TEntity entity);

        bool Update(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        bool Delete(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        bool Delete(object id);

        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter);

        bool Delete(Expression<Func<TEntity, bool>> filter);

        Task<bool> DeleteAsync(object id);

        Task<bool> Any();

        Task<bool> AnyAsync();

        Task<bool> Any(Expression<Func<TEntity, bool>> filter);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);

        int Count();

        int Count(Expression<Func<TEntity, bool>> filter);

        Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> sum);

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter);

        TEntity Get(Expression<Func<TEntity, bool>> filter);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null);

        TEntity LastOrDefault(Expression<Func<TEntity, bool>> filter = null);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter);

        Task<IQueryable<TEntity>> GetAllAsync();

        Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetById(object id);

        Task<TEntity> GetByIdAsync(object id);
    }
}
