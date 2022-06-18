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

       
        Task<bool> UpdateAsync(TEntity entity);

       int Count();

        Task<IQueryable<TEntity>> GetAllAsync();

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties);

    }
}
