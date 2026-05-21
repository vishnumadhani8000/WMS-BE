using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("carts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.TotalWeightKg).IsRequired().HasColumnType("decimal(10,3)");

        builder.Property(x => x.IsCheckedOut).IsRequired().HasDefaultValue(false);

        builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("now()");

        builder.Property(x => x.DeletedAt).IsRequired(false);

        builder.Property(x => x.CreatedBy).IsRequired(false);

        builder.Property(x => x.UpdatedBy).IsRequired(false);

        builder.Property(x => x.DeletedBy).IsRequired(false);

        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.HasIndex(x => x.UserId).HasDatabaseName("carts_user_id_idx");

        builder.HasOne(x => x.User).WithMany(u => u.Carts).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UpdatedByUser).WithMany().HasForeignKey(x => x.UpdatedBy).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DeletedByUser).WithMany().HasForeignKey(x => x.DeletedBy).OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}