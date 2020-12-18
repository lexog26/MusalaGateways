using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusalaGateways.Domain.Entities;
using MusalaGateways.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaGateways.DataLayer.EntityConfigs
{
    public class DeviceConfig
    {
        public DeviceConfig(EntityTypeBuilder<Device> entity)
        {
            entity.ToTable("Devices");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.Uid).IsRequired();
            entity.Property(p => p.Vendor).IsRequired(false).HasMaxLength(100);
            entity.Property(p => p.Status).IsRequired(true).HasDefaultValue(DeviceStatus.Offline);


            entity.Property(p => p.LastTimeStamp).IsRequired().ValueGeneratedOnAddOrUpdate();
            entity.Property(p => p.ModifiedDate).IsRequired(false).ValueGeneratedOnUpdate();
            entity.Property(p => p.CreatedDate).IsRequired().ValueGeneratedOnAdd();


            entity.HasOne(p => p.Gateway)
                  .WithMany(g => g.Devices)
                  .HasForeignKey(p => p.GatewayId)
                  .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
