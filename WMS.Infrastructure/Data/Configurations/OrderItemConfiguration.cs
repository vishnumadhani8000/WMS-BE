using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.Quantity)      .IsRequired();
        builder.Property(x => x.UnitWeightKg)  .IsRequired().HasColumnType("decimal(10,3)");
        builder.Property(x => x.LineWeightKg)  .IsRequired().HasColumnType("decimal(12,3)");
        builder.Property(x => x.CreatedAt)     .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.DeletedAt)     .IsRequired(false);

        builder.HasIndex(x => x.OrderId)   .HasDatabaseName("order_items_order_id_idx");
        builder.HasIndex(x => x.ProductId) .HasDatabaseName("order_items_product_id_idx");

        builder.HasOne(x => x.Order)  .WithMany(o => o.OrderItems).HasForeignKey(x => x.OrderId)  .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Product).WithMany(p => p.OrderItems).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}
