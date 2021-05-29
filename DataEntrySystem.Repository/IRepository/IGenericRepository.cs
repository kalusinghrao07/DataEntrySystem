using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataEntrySystem.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? pageIndex = null,
            int? pageSize = null,
            string includeProperties = "");
        TEntity GetByID(object id);
        IQueryable<TEntity> GetAsNoTracking();
        Task<TEntity> GetByIDAsync(object id);
        void Insert(TEntity entity);
        Task<bool> InsertAsync(TEntity entity);
        void Delete(object id);
        Task<bool> DeleteAsync(object id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        Task<int> RemoveByExpression(Expression<Func<TEntity, bool>> expr);
        Task<IEnumerable<TEntity>> BulkAdd(IEnumerable<TEntity> entity);
        Task<IEnumerable<TEntity>> BulkUpdate(IEnumerable<TEntity> entity);
    }
}
