using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Web.Data.Repositories
{
    public interface IGenericRepository<TEntity, TId> where TEntity : class, IEntity<TId>
    {
        Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = false);
        Task<List<TEntity>> GetAll(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        IQueryable<TEntity> GetAllAsIQueryable();
        Task<List<TEntity>> GetAllAsNoTracking();
        Task<TEntity> GetById(TId id);
        Task<TEntity> GetByIdWithInclude(TId id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        Task Insert(TEntity entity);
        Task InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entityToUpdate);
        void Delete(TEntity entityToDelete);
        Task Delete(TId id);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> include)
            where TProperty : class;
        Task LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> include)
           where TProperty : class;
    }
}
