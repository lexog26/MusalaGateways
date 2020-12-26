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

        public IEnumerable<TEntity> GetAll<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            return GetQueryable(
                orderBy: orderBy,
                skip: skip,
                take: take,
                noTracking: noTracking,
                includeProperties: includeProperties);
        }

        public IEnumerable<TSelectType> GetAll<TEntity, TSelectType>(
            Expression<Func<TEntity, TSelectType>> selector,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            return GetQueryable(
                selector: selector,
                orderBy: orderBy,
                skip: skip,
                take: take,
                noTracking: noTracking,
                includeProperties: includeProperties);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            var query = GetQueryable(
                orderBy: orderBy,
                skip: skip,
                take: take,
                noTracking: noTracking,
                includeProperties: includeProperties);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TSelectType>> GetAllAsync<TEntity, TSelectType>(
            Expression<Func<TEntity, TSelectType>> selector,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            var query = GetQueryable(
                selector: selector,
                orderBy: orderBy,
                skip: skip,
                take: take,
                noTracking: noTracking,
                includeProperties: includeProperties);
            return await query.ToListAsync();
        }

        public IEnumerable<TSelectType> GetByFilter<TEntity, TSelectType>(
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, TSelectType>> selector = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            return GetQueryable(
                selector: selector,
                filter: filter,
                orderBy: orderBy,
                skip: skip,
                take: take,
                noTracking: noTracking,
                includeProperties: includeProperties);
        }

        public async Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            var query = GetQueryable(
                filter: filter,
                orderBy: orderBy,
                skip: skip,
                take: take,
                noTracking: noTracking,
                includeProperties: includeProperties);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TSelectType>> GetByFilterAsync<TEntity, TSelectType>(
            Expression<Func<TEntity, TSelectType>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            var query = GetQueryable(
                filter: filter,
                selector: selector,
                orderBy: orderBy,
                skip: skip,
                take: take,
                noTracking: noTracking,
                includeProperties: includeProperties);
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

        public TEntity GetEntityById<TEntity, TKey>(TKey id, bool noTracking = false, params string[] includeProperties)
            where TEntity : Entity<TKey>
        {
            if (includeProperties == null || includeProperties.Length == 0)
            {
                var entity = _context.Find<TEntity>(id);
                if (entity != null && noTracking)
                {
                    _context.Entry(entity).State = EntityState.Detached;
                }
                return entity;
            }
            return GetFirstOrDefault<TEntity>(
                filter: x => x.Id.Equals(id),
                noTracking: noTracking,
                includeProperties: includeProperties
                );
        }

        public Task<TEntity> GetEntityByIdAsync<TEntity, TKey>(TKey id, bool noTracking = false, params string[] includeProperties)
            where TEntity : Entity<TKey>
        {
            return Task.FromResult(GetEntityById<TEntity, TKey>(id, noTracking, includeProperties));
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

        public TEntity GetFirstOrDefault<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            return GetQueryable(
                filter: filter,
                orderBy: orderBy,
                noTracking: noTracking,
                includeProperties: includeProperties
                ).FirstOrDefault();
        }

        public TSelectType GetFirstOrDefault<TEntity, TSelectType>(
            Expression<Func<TEntity, TSelectType>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            return GetQueryable(
                filter: filter,
                selector: selector,
                orderBy: orderBy,
                noTracking: noTracking,
                includeProperties: includeProperties).FirstOrDefault();
        }

        public async Task<TEntity> GetFirstOrDefaultAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            return await GetQueryable(
                filter: filter,
                orderBy: orderBy,
                noTracking: noTracking,
                includeProperties: includeProperties).FirstOrDefaultAsync();
        }

        public async Task<TSelectType> GetFirstOrDefaultAsync<TEntity, TSelectType>(
            Expression<Func<TEntity, TSelectType>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            return await GetQueryable(
                filter: filter,
                selector: selector,
                orderBy: orderBy,
                noTracking: noTracking,
                includeProperties: includeProperties).FirstOrDefaultAsync();
        }



        #region Queries generator

        /// <summary>
        /// Creates a TEntity IQueryable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter">IQueryable filter</param>
        /// <param name="orderBy">IQueryable order by</param>
        /// <param name="skip">Number of elements to skip in resulting IQueryable</param>
        /// <param name="take">Number of elements to take in resulting IQueryable</param>
        /// <param name="noTracking"></param>
        /// Specify the track performance using the queryable.AsNoTracking() method
        /// noTracking = true => the change tracker will not track any returned entity by EF
        /// default noTracking = false
        /// <param name="includeProperties">Navigation properties to include</param>
        /// <returns></returns>
        protected virtual IQueryable<TSelectType> GetQueryable<TEntity, TSelectType>(
            Expression<Func<TEntity, TSelectType>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            IQueryable<TEntity> query = GetQueryable(
                filter: filter,
                skip: skip,
                take: take,
                orderBy: orderBy,
                noTracking: noTracking,
                includeProperties: includeProperties
            );

            return query.Select(selector);
        }


        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            bool noTracking = false,
            params string[] includeProperties)
            where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }
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

            return noTracking ? query.AsNoTracking() : query;
        }
        #endregion
    }
}
