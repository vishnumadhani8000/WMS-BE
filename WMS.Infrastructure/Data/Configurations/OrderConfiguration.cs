using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.Status)        .IsRequired().HasConversion<string>();
        builder.Property(x => x.TotalWeightKg)    .IsRequired().HasColumnType("decimal(10,3)");
        builder.Property(x => x.Notes)         .IsRequired(false);
        builder.Property(x => x.ShipmentId)    .IsRequired(false);
        builder.Property(x => x.CreatedAt)     .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.DeletedAt)     .IsRequired(false);
        builder.Property(x => x.CreatedBy)     .IsRequired(false);
        builder.Property(x => x.UpdatedBy)     .IsRequired(false);
        builder.Property(x => x.DeletedBy)     .IsRequired(false);
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.TotalPrice).HasPrecision(18, 2).IsRequired();

        builder.HasIndex(x => x.UserId)     .HasDatabaseName("orders_user_id_idx");
        builder.HasIndex(x => x.CartId)         .HasDatabaseName("orders_cart_id_idx");
        builder.HasIndex(x => x.AddressId)  .HasDatabaseName("orders_address_id_idx");
        builder.HasIndex(x => x.ShipmentId) .HasDatabaseName("orders_shipment_id_idx");
        builder.HasIndex(x => x.Status)     .HasDatabaseName("orders_status_idx");
        builder.HasIndex(x => x.DeletedAt)  .HasDatabaseName("orders_deleted_at_idx");

        builder.HasOne(x => x.User)    .WithMany(u => u.Orders)         .HasForeignKey(x => x.UserId)    .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Address) .WithMany(a => a.Orders)         .HasForeignKey(x => x.AddressId) .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Shipment).WithMany(s => s.Orders)         .HasForeignKey(x => x.ShipmentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.UpdatedByUser).WithMany().HasForeignKey(x => x.UpdatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.DeletedByUser).WithMany().HasForeignKey(x => x.DeletedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Cart).WithMany().HasForeignKey(x => x.CartId).OnDelete(DeleteBehavior.Restrict);

       builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
