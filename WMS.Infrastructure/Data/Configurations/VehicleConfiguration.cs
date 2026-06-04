using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.Name)        .IsRequired().HasMaxLength(150);
        builder.Property(x => x.PlateNumber) .IsRequired().HasMaxLength(30);
        builder.Property(x => x.CapacityKg)  .IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(x => x.IsAvailable) .IsRequired().HasDefaultValue(true);
        builder.Property(x => x.CreatedAt)   .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.DeletedAt)   .IsRequired(false);
        builder.Property(x => x.CreatedBy)   .IsRequired(false);
        builder.Property(x => x.UpdatedBy)   .IsRequired(false);
        builder.Property(x => x.DeletedBy)   .IsRequired(false);
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.HasIndex(x => x.PlateNumber).IsUnique().HasDatabaseName("vehicles_plate_uidx");
        builder.HasIndex(x => x.IsAvailable).HasDatabaseName("vehicles_available_idx");
        builder.HasIndex(x => x.DeletedAt)  .HasDatabaseName("vehicles_deleted_at_idx");

        builder.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.UpdatedByUser).WithMany().HasForeignKey(x => x.UpdatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.DeletedByUser).WithMany().HasForeignKey(x => x.DeletedBy).OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
