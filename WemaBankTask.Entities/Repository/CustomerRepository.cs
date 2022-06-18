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

        private async Task<bool> Commit(bool isAsync)
        {
            try
            {
                if (!isBatch) return false;
                if (isAsync)
                    await this.SaveAsync(null);
                else
                    this.Save();
                this.Context.Database.CommitTransaction();
                isBatch = false;
                return true;
            }
            catch (Exception)
            {
                // log error
                this.RollbackTransaction();
                return false;
            }
        }

        public void RollbackTransaction()
        {
            this.Context.Database.RollbackTransaction();
        }

        public async Task<bool> Any()
        {
            return await this.Entity.AnyAsync();
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> filter)
        {
            return await this.Entity.AnyAsync(filter);
        }

        public async Task<bool> AnyAsync()
        {
            return await this.Entity.AnyAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await this.Entity.AnyAsync(filter);
        }

        public async Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> sum)
        {
            return await this.Entity.SumAsync(sum);
        }

        public int Count()
        {
            return this.Entity.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            return this.Entity.Count(filter);
        }

        public async Task<int> CountAsync()
        {
            return await this.Entity.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await this.Entity.CountAsync(filter);
        }

        public TEntity GetById(object id)
        {
            return this.Entity.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await this.Entity.FindAsync(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return this.Entity.FirstOrDefault(filter);
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

        public IQueryable<TEntity> GetAll()
        {
            return this.Entity.AsQueryable<TEntity>();
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter)
        {
            return this.Entity.Where(filter).AsQueryable<TEntity>();
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Task.FromResult(this.Entity.Where(filter).AsQueryable<TEntity>());
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

        public bool Save()
        {
            ChangeCount = this.Context.SaveChanges();
            return ChangeCount > 0;
        }

        public bool Remove(TEntity entity)
        {
            UpdateEntityState(entity, EntityState.Detached);
            this.Context.Remove(entity);
            return Save();
        }

        public bool Remove(object id)
        {
            var entity = this.Entity.Find(id);
            if (entity == null) return false;

            UpdateEntityState(entity, EntityState.Detached);
            this.Context.Remove(entity);
            return Save();
        }

        public bool AddRange(IEnumerable<TEntity> entity)
        {
            this.Entity.AddRange(entity);
            if (isBatch) return true;
            return Save();
        }

        public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entity)
        {
            await this.Entity.AddRangeAsync(entity);
            if (isBatch) return true;
            return await SaveAsync(null);
        }

        public bool Add(TEntity entity)
        {
            this.Entity.Add(entity);
            if (isBatch) return true;
            return Save();
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            await this.Entity.AddAsync(entity);
            if (isBatch) return true;
            return await SaveAsync(entity);
        }

        public async Task<int> AddAsyncReturnsId(TEntity entity)
        {
            await this.Entity.AddAsync(entity);
            return await SaveAsyncReturnsId();
        }

        public bool Update(TEntity entity)
        {
            UpdateEntityState(entity, EntityState.Modified);
            return Save();
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            UpdateEntityState(entity, EntityState.Modified);
            return await SaveAsync(entity);
        }

        public bool Delete(TEntity entity)
        {
            if (entity == null) return false;

            UpdateEntityState(entity, EntityState.Deleted);
            return Save();
        }

        public bool Delete(object id)
        {
            var entity = this.Entity.Find(id);
            if (entity == null) return false;

            UpdateEntityState(entity, EntityState.Deleted);
            if (isBatch) return true;
            return this.Save();
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            UpdateEntityState(entity, EntityState.Deleted);
            if (isBatch) return true;
            return await this.SaveAsync(entity);
        }

        public async Task<bool> DeleteAsync(object id)
        {
            var entity = this.Entity.Find(id);
            if (entity == null) return false;

            UpdateEntityState(entity, EntityState.Deleted);
            if (isBatch) return true;
            return await SaveAsync(entity);
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            var entity = this.Entity.FirstOrDefault(filter);
            if (entity == null) return false;

            UpdateEntityState(entity, EntityState.Deleted);
            if (isBatch) return true;
            return await this.SaveAsync(entity);
        }

        public bool Delete(Expression<Func<TEntity, bool>> filter)
        {
            var entity = this.Entity.FirstOrDefault(filter);
            if (entity == null) return false;

            UpdateEntityState(entity, EntityState.Deleted);
            if (isBatch) return true;
            return this.Save();
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

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
                return this.Entity.FirstOrDefault();
            else
                return this.Entity.FirstOrDefault(filter);
        }

        public TEntity LastOrDefault(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
                return this.Entity.LastOrDefault();
            else
                return this.Entity.LastOrDefault(filter);
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = Context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

    }
}
