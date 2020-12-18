using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusalaGateways.Domain.Entities;

namespace MusalaGateways.DataLayer.EntityConfigs
{
    public class GatewayConfig
    {
        public GatewayConfig(EntityTypeBuilder<Gateway> entity)
        {
            entity.ToTable("Gateways");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.Name).IsRequired(false).HasMaxLength(50);
            entity.Property(p => p.SerialNumber).IsRequired().HasMaxLength(255);
            entity.Property(p => p.Ipv4Address).IsRequired().HasMaxLength(255);

            entity.Property(p => p.LastTimeStamp).IsRequired().ValueGeneratedOnAddOrUpdate();
            entity.Property(p => p.ModifiedDate).IsRequired(false).ValueGeneratedOnUpdate();
            entity.Property(p => p.CreatedDate).IsRequired().ValueGeneratedOnAdd();

            entity.HasIndex(p => p.SerialNumber).IsUnique();

            entity.HasMany(p => p.Devices)
                  .WithOne(d => d.Gateway);

        }
    }
}
