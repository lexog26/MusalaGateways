using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MusalaGateways.DataLayer.EntityConfigs;
using MusalaGateways.Domain.Entities;
using System.IO;

namespace SurgerySimulator.DataLayer.Context
{
    public partial class MusalaContext : DbContext
    {
        public MusalaContext(DbContextOptions<MusalaContext> options) : base(options)
        { }

        public MusalaContext() : base()
        { }

        #region DbSets

        public DbSet<Gateway> Gateways { get; set; }

        public DbSet<Device> Devices { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new GatewayConfig(builder.Entity<Gateway>());
            new DeviceConfig(builder.Entity<Device>());
        }

    }
}
