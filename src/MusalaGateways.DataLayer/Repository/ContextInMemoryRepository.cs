using Microsoft.EntityFrameworkCore;
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
            var gateways = GetAll<Gateway>().ToList();
            var devices = GetAll<Device>().ToList();
            return base.GetEntityByIdAsync<TEntity, TKey>(id, noTracking);
        }

    }
}
