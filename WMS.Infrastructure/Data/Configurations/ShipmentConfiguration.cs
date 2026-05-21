using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("shipments");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.Status)        .IsRequired().HasConversion<string>();
        builder.Property(x => x.TotalWeightKg) .IsRequired().HasColumnType("decimal(12,3)");
        builder.Property(x => x.DispatchedAt)  .IsRequired(false);
        builder.Property(x => x.DeliveredAt)   .IsRequired(false);
        builder.Property(x => x.Notes)         .IsRequired(false);
        builder.Property(x => x.CreatedAt)     .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.DeletedAt)     .IsRequired(false);
        builder.Property(x => x.CreatedBy)     .IsRequired(false);
        builder.Property(x => x.UpdatedBy)     .IsRequired(false);
        builder.Property(x => x.DeletedBy)     .IsRequired(false);
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.HasIndex(x => x.DriverId)  .HasDatabaseName("shipments_driver_id_idx");
        builder.HasIndex(x => x.VehicleId) .HasDatabaseName("shipments_vehicle_id_idx");
        builder.HasIndex(x => x.Status)    .HasDatabaseName("shipments_status_idx");
        builder.HasIndex(x => x.DeletedAt) .HasDatabaseName("shipments_deleted_at_idx");

        builder.HasOne(x => x.Driver) .WithMany(d => d.Shipments).HasForeignKey(x => x.DriverId) .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Vehicle).WithMany(v => v.Shipments).HasForeignKey(x => x.VehicleId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.UpdatedByUser).WithMany().HasForeignKey(x => x.UpdatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.DeletedByUser).WithMany().HasForeignKey(x => x.DeletedBy).OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
