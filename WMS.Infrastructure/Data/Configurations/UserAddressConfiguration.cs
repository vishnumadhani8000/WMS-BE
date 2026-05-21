using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.ToTable("user_addresses");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.AddressLine).IsRequired();
        builder.Property(x => x.Landmark)   .HasMaxLength(255);
        builder.Property(x => x.Pincode)    .IsRequired().HasMaxLength(10);
        builder.Property(x => x.IsDefault)  .IsRequired().HasDefaultValue(false);
        builder.Property(x => x.CreatedAt)  .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.DeletedAt)  .IsRequired(false);
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.HasIndex(x => x.UserId)    .HasDatabaseName("user_addresses_user_id_idx");
        builder.HasIndex(x => x.CityId)    .HasDatabaseName("user_addresses_city_id_idx");
        builder.HasIndex(x => x.StateId)   .HasDatabaseName("user_addresses_state_id_idx");
        builder.HasIndex(x => x.DeletedAt) .HasDatabaseName("user_addresses_deleted_at_idx");

        builder.HasOne(x => x.User) .WithMany(u => u.Addresses).HasForeignKey(x => x.UserId) .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.State).WithMany(s => s.Addresses).HasForeignKey(x => x.StateId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.City) .WithMany(c => c.Addresses).HasForeignKey(x => x.CityId) .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
