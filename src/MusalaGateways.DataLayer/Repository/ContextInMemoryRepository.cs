﻿using Microsoft.EntityFrameworkCore;
using MusalaGateways.DataLayer.Repository.Interface;
using MusalaGateways.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusalaGateways.DataLayer.Repository
{
    public class ContextInMemoryRepository<TContext> : ContextRepository<TContext>, IRepository where TContext : DbContext
    {
        public ContextInMemoryRepository(TContext context) : base(context)
        {
        }

        public override Task<TEntity> GetEntityByIdAsync<TEntity, TKey>(TKey id, bool noTracking = false)
        {
            var inMemory = GetAll<TEntity>().ToList();
            return base.GetEntityByIdAsync<TEntity, TKey>(id, noTracking);
        }

        public override TKey Create<TEntity, TKey>(TEntity entity)
        {
            var inMemory = GetAll<TEntity>().ToList();
            return base.Create<TEntity, TKey>(entity);
        }

        public override IEnumerable<TKey> Create<TEntity, TKey>(IEnumerable<TEntity> entities)
        {
            var inMemory = GetAll<TEntity>().ToList();
            return base.Create<TEntity, TKey>(entities);
        }

        public override TEntity Delete<TEntity, TKey>(TKey id)
        {
            var inMemory = GetAll<TEntity>().ToList();
            return base.Delete<TEntity, TKey>(id);
        }

        public override bool Delete<TEntity>(TEntity entity)
        {
            var inMemory = GetAll<TEntity>().ToList();
            return base.Delete(entity);
        }
    }
}