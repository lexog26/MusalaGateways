using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.Domain.Entities;

namespace MusalaGateways.DataLayer.Repository
{
    /// <summary>
    /// Implement's IRepository based on DbContext
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ContextRepository<TContext> : IRepository where TContext : DbContext
    {
        protected readonly TContext _context;

        public ContextRepository(TContext context) 
        {
            _context = context;
        }

        #region Create

        public TKey Create<TEntity, TKey>(TEntity entity) where TEntity : Entity<TKey>
        {
            entity.CreatedDate = DateTime.UtcNow;
            _context.Add(entity);
            return entity.Id;
        }

        public IEnumerable<TKey> Create<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : Entity<TKey>
        {
            foreach (var entity in entities)
            {
                yield return Create<TEntity, TKey>(entity);
            }
        }

        #endregion

        #region Delete

        public TEntity Delete<TEntity, TKey>(TKey id) where TEntity : Entity<TKey>
        {
            var entity = _context.Find<TEntity>(id);
            Delete(entity);
            return entity;
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity != null)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _context.Attach(entity);
                }
                _context.Remove(entity);
                return true;
            }
            return false;
        }

        public bool Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                if (!Delete(entity))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Update

        public bool Update<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Update(entity);
            return true;
        }

        #endregion

        public IEnumerable<TEntity> GetAll<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, bool noTracking = false)
            where TEntity : class
        {
            return GetQueryable(orderBy: orderBy, skip: skip, take: take, noTracking: noTracking).OfType<TEntity>();
        }

        public IEnumerable<object> GetAll<TEntity>(Expression<Func<TEntity, object>> selector, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, bool noTracking = false)
            where TEntity : class
        {
            return GetQueryable(orderBy: orderBy, skip: skip, take: take, noTracking: noTracking, selector: selector);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, bool noTracking = false)
            where TEntity : class
        {
            var query = GetQueryable(orderBy: orderBy, skip: skip, take: take, noTracking: noTracking).OfType<TEntity>();
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetAllAsync<TEntity>(Expression<Func<TEntity, object>> selector, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, bool noTracking = false)
            where TEntity : class
        {
            var query = GetQueryable(selector: selector, orderBy: orderBy, skip: skip, take: take, noTracking: noTracking);
            return await query.ToListAsync();
        }

        public IEnumerable<TEntity> GetByFilter<TEntity>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> selector = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, bool noTracking = false)
            where TEntity : class
        {
            return GetQueryable(selector: selector, filter: filter, orderBy: orderBy, skip: skip, take: take, noTracking: noTracking).OfType<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, bool noTracking = false)
            where TEntity : class
        {
            var query = GetQueryable(filter: filter, orderBy: orderBy, skip: skip, take: take, noTracking: noTracking).OfType<TEntity>();
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetByFilterAsync<TEntity>(Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, bool>> filter = null,  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, bool noTracking = false)
            where TEntity : class
        {
            var query = GetQueryable(selector: selector, filter: filter, orderBy: orderBy, skip: skip, take: take, noTracking: noTracking).OfType<TEntity>();
            return await query.ToListAsync();
        }

        public int GetCount<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            return GetQueryable(filter: filter).Count();
        }

        public async Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class
        {
            return await GetQueryable(filter: filter).CountAsync();
        }

        public TEntity GetEntityById<TEntity,TKey>(TKey id, bool noTracking = false)
            where TEntity : Entity<TKey>
        {
            var entity = _context.Find<TEntity>(id);
            if (entity != null && noTracking)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public object GetEntityById<TEntity, TKey>(TKey id, Expression<Func<TEntity, object>> selector, bool noTracking = false)
            where TEntity : Entity<TKey>
        {
            return GetFirst(filter: x => x.Id.Equals(id), noTracking: noTracking, selector: selector);
        }

        public async Task<TEntity> GetEntityByIdAsync<TEntity, TKey>(TKey id, bool noTracking = false)
            where TEntity : Entity<TKey>
        {
            var entity = await _context.FindAsync<TEntity>(id);
            if (noTracking)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public async Task<object> GetEntityByIdAsync<TEntity, TKey>(TKey id, Expression<Func<TEntity, object>> selector, bool noTracking = false)
            where TEntity : Entity<TKey>
        {
            return await GetFirstAsync(selector: selector, filter: x => x.Id.Equals(id), noTracking: noTracking);
        }

        public bool GetExists<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            return GetQueryable(filter: filter).Any();
        }

        public async Task<bool> GetExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class
        {
            return await GetQueryable(filter: filter).AnyAsync();
        }

        public TEntity GetFirst<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool noTracking = false)
            where TEntity : class
        {
            return GetQueryable(filter: filter, orderBy: orderBy, noTracking: noTracking).OfType<TEntity>().FirstOrDefault();
        }

        public object GetFirst<TEntity>(Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool noTracking = false)
            where TEntity : class
        {
            return GetQueryable(filter: filter, selector: selector, orderBy: orderBy, noTracking: noTracking).FirstOrDefault();
        }

        public async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool noTracking = false)
            where TEntity : class
        {
            var query = GetQueryable(filter: filter, orderBy: orderBy, noTracking: noTracking).OfType<TEntity>();
            return await query.FirstOrDefaultAsync();
        }

        public async Task<object> GetFirstAsync<TEntity>(Expression<Func<TEntity, object>> selector, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, bool noTracking = false)
            where TEntity : class
        {
            var query = GetQueryable(filter: filter, selector: selector, orderBy: orderBy, noTracking: noTracking);
            return await query.FirstOrDefaultAsync();
        }

        

        #region Queries generator

        /// <summary>
        /// Creates a Queryable
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="noTracking">
        /// Specify the track performance using the queryable.AsNoTracking() method
        /// noTracking=true => the change tracker will not track any returned entity by EF
        /// default noTracking = false
        /// </param>
        /// <param name="selector"></param>
        /// <returns></returns>
        protected virtual IQueryable<object> GetQueryable<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool noTracking = false,
            Expression<Func<TEntity, object>> selector = null) where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            IQueryable<object> result = query;

            if (selector != null)
            {
                result = query.Select(selector);
            }

            return noTracking ? result.AsNoTracking() : result;
        }

        #endregion
    }
}
