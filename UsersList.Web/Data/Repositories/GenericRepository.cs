using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Web.Data.Repositories
{
    internal class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : class, IEntity<TId>
    {
        protected readonly ApplicationContext _applicationContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _dbSet = applicationContext.Set<TEntity>();
        }

        public virtual Task<List<TEntity>> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            return (include != null) ? include(_dbSet).ToListAsync() : _dbSet.ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAllAsNoTracking()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual IQueryable<TEntity> GetAllAsIQueryable()
        {
            return _dbSet;
        }

        public virtual Task<List<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = false)
        {
            IQueryable<TEntity> query = _dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToListAsync();
            }
            else
            {
                return query.ToListAsync();
            }
        }

        public virtual async Task<TEntity> GetById(TId id)
        {
            return await _dbSet.SingleOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        public Task<TEntity> GetByIdWithInclude(TId id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            return query.SingleOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        public virtual async Task Insert(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task InsertRange(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task Delete(TId id)
        {
            Delete(await GetById(id));
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_applicationContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            if (_applicationContext.Entry(entityToUpdate).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToUpdate);
            }
            _applicationContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.AnyAsync(predicate);
        }

        public Task LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> include)
            where TProperty : class
        {
            return _applicationContext.Entry(entity).Reference(include).LoadAsync();
        }

        public Task LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> include)
            where TProperty : class
        {
            return _applicationContext.Entry(entity).Collection(include).LoadAsync();
        }
    }
}
