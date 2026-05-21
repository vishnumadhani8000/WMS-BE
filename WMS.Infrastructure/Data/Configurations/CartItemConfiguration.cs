using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("cart_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.Quantity).IsRequired();

        builder.Property(x => x.WeightKg).IsRequired().HasColumnType("decimal(10,3)");

        builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("now()");

        builder.Property(x => x.DeletedAt).IsRequired(false);

        builder.Property(x => x.CreatedBy).IsRequired(false);

        builder.Property(x => x.UpdatedBy).IsRequired(false);

        builder.Property(x => x.DeletedBy).IsRequired(false);

        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.HasIndex(x => new { x.CartId, x.ProductId }).IsUnique().HasFilter("\"IsDeleted\" = false").HasDatabaseName("cart_items_cart_product_uidx");

        builder.HasIndex(x => x.CartId).HasDatabaseName("cart_items_cart_id_idx");

        builder.HasIndex(x => x.ProductId).HasDatabaseName("cart_items_product_id_idx");

        builder.HasOne(x => x.Cart).WithMany(c => c.CartItems).HasForeignKey(x => x.CartId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Product).WithMany(p => p.cartItems).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);


        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}