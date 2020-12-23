using Microsoft.EntityFrameworkCore;
using MusalaGateways.DataLayer.EntityConfigs;
using MusalaGateways.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaGateways.DataLayer.Context
{
    public class MusalaInMemoryContext : DbContext
    {
        public MusalaInMemoryContext(DbContextOptions<MusalaContext> options) : base(options)
        { }

        public MusalaInMemoryContext() : base()
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

            Gateways.Add(
                new Gateway
                {
                    Id = 1,
                    Ipv4Address = "192.34.28.54",
                    Name = "Gateway 1",
                    SerialNumber = "472"
                });
            Devices.Add(
                new Device
                {
                    Id = 1,
                    GatewayId = 1,
                    Status = Enums.DeviceStatus.Online,
                    CreatedDate = DateTime.UtcNow,
                    Vendor = "Vendor",
                    Uid = 12
                });
            SaveChanges();
        }
    }
}
