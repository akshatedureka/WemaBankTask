using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WemaBankTask.Entities.DataContext;
using WemaBankTask.Entities.IRepository;
using WemaBankTask.Entities.Model;

namespace WemaBankTask.Entities.Repository
{
    public class BaseManager<TEntity> : CustomerRepository<TEntity> where TEntity : class
    {
        public BaseManager(DbContext context) : base(context)
        {
        }
    }
    public class CustomerRepository<TEntity> : ICustomerRepository<TEntity> where TEntity : class
    {
        internal DbContext Context;
        internal DbSet<TEntity> Entity;

        public int ChangeCount { get; set; }
        private bool isBatch;

        public CustomerRepository(DbContext context)
        {
            this.Context = context;
            this.Entity = this.Context.Set<TEntity>();
        }

       

        

        public int Count()
        {
            return this.Entity.Count();
        }
       
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = Entity;

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(filter);
        }


        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            return await Task.FromResult(this.Entity.AsQueryable<TEntity>());
        }

        public async Task<bool> SaveAsync(TEntity entity)
        {
            ChangeCount = await this.Context.SaveChangesAsync();
            if (entity != null)
                this.Context.Entry(entity).State = EntityState.Detached;
            return ChangeCount > 0;
        }

        public async Task<int> SaveAsyncReturnsId()
        {
            return await this.Context.SaveChangesAsync();
        }

        public async Task<int> AddAsyncReturnsId(TEntity entity)
        {
            await this.Entity.AddAsync(entity);
            return await SaveAsyncReturnsId();
        }

        

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            UpdateEntityState(entity, EntityState.Modified);
            return await SaveAsync(entity);
        }


        public void UpdateEntityState(TEntity entity, EntityState entityState)
        {
            var dbEntityEntry = GetDbEntityEntry(entity);
            dbEntityEntry.State = entityState;
        }

        public EntityEntry GetDbEntityEntry(TEntity entity)
        {
            var dbEntityEntry = this.Context.Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                this.Context.Set<TEntity>().Attach(entity);
            }
            return dbEntityEntry;
        }


    }
}
