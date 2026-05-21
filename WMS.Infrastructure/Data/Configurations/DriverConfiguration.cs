using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("drivers");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.Name)        .IsRequired().HasMaxLength(120);
        builder.Property(x => x.Phone)       .IsRequired().HasMaxLength(20);
        builder.Property(x => x.LicenceNo)   .HasMaxLength(50);
        builder.Property(x => x.IsAvailable) .IsRequired().HasDefaultValue(true);
        builder.Property(x => x.CreatedAt)   .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.DeletedAt)   .IsRequired(false);
        builder.Property(x => x.CreatedBy)   .IsRequired(false);
        builder.Property(x => x.UpdatedBy)   .IsRequired(false);
        builder.Property(x => x.DeletedBy)   .IsRequired(false);
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.HasIndex(x => x.IsAvailable).HasDatabaseName("drivers_available_idx");
        builder.HasIndex(x => x.DeletedAt)  .HasDatabaseName("drivers_deleted_at_idx");

        builder.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.UpdatedByUser).WithMany().HasForeignKey(x => x.UpdatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.DeletedByUser).WithMany().HasForeignKey(x => x.DeletedBy).OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
