using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.Name)        .IsRequired().HasMaxLength(200);
        builder.Property(x => x.WeightKg)    .IsRequired().HasColumnType("decimal(10,3)");
        builder.Property(x => x.Stock)       .IsRequired().HasDefaultValue(0);
        builder.Property(x => x.Description) .IsRequired(false);
        builder.Property(x => x.CreatedAt)   .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.DeletedAt)   .IsRequired(false);
        builder.Property(x => x.CreatedBy)   .IsRequired(false);
        builder.Property(x => x.UpdatedBy)   .IsRequired(false);
        builder.Property(x => x.DeletedBy)   .IsRequired(false);
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.HasIndex(x => x.Name)      .HasDatabaseName("products_name_idx");
        builder.HasIndex(x => x.DeletedAt) .HasDatabaseName("products_deleted_at_idx");

        builder.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.UpdatedByUser).WithMany().HasForeignKey(x => x.UpdatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.DeletedByUser).WithMany().HasForeignKey(x => x.DeletedBy).OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
