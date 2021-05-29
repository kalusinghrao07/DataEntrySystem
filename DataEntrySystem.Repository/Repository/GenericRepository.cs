using DataEntrySystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataEntrySystem.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DocumentDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DocumentDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();

        }
        /// <summary>
        /// Get Data with expression filter and order with delegates 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">orderBy and pageSize is mandatory with this param.</param>
        /// <param name="pageSize">orderBy and pageIndex is mandatory with this param.</param>
        /// <param name="includeProperties"></param>
        /// <returns>returns IEnumerable List of TEntity Type</returns>
        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? pageIndex = null,
            int? pageSize = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
                if (pageIndex != null && pageSize != null)
                {
                    query = query.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
            }

            return query;

        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }
        public virtual IQueryable<TEntity> GetAll(object id)
        {
            IQueryable<TEntity> query = dbSet;
            return query;
        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual IQueryable<TEntity> GetAsNoTracking()
        {
            return dbSet.AsNoTracking();
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual async Task<bool> DeleteAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
                return true;
            }
            return false;
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.SaveChanges();
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        public virtual void UpdateChilds(TEntity entityToUpdate)
        {
            context.Entry(entityToUpdate).CurrentValues.SetValues(entityToUpdate);
        }

        public virtual async Task<int> RemoveByExpression(Expression<Func<TEntity, bool>> expr)
        {
            return await Task.Run(() =>
            {
                var entities = dbSet.Where(expr).ToList();
                if (entities.Any())
                {
                    foreach (var entity in entities)
                    {
                        dbSet.Remove(entity);
                    }
                }
                return context.SaveChanges();
            });
        }

        public async Task<IEnumerable<TEntity>> BulkAdd(IEnumerable<TEntity> entity)
        {

            if (entity == null)
                throw new ArgumentException("entity is null");

            return await Task.Run(() =>
            {
                try
                {
                    context.Set<TEntity>().AddRange(entity);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw;
                }
                return entity;
            });

        }

        public async Task<IEnumerable<TEntity>> BulkUpdate(IEnumerable<TEntity> entity)
        {

            if (entity == null)
                throw new ArgumentException("entity is null");

            return await Task.Run(() =>
            {

                context.Set<TEntity>().UpdateRange(entity);
                context.SaveChanges();

                return entity;
            });

        }
    }
}
