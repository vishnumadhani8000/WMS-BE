using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.Name)         .IsRequired().HasMaxLength(120);
        builder.Property(x => x.Email)        .IsRequired().HasMaxLength(255);
        builder.Property(x => x.PasswordHash) .IsRequired().HasMaxLength(255);
        builder.Property(x => x.Role)         .IsRequired().HasConversion<string>();
        builder.Property(x => x.Phone)        .HasMaxLength(20);
        builder.Property(x => x.IsActive)     .IsRequired().HasDefaultValue(true);
        builder.Property(x => x.CreatedAt)    .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedAt)    .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.DeletedAt)    .IsRequired(false);

        builder.HasIndex(x => x.Email)     .IsUnique().HasDatabaseName("users_email_uidx");
        builder.HasIndex(x => x.Role)      .HasDatabaseName("users_role_idx");
        builder.HasIndex(x => x.DeletedAt) .HasDatabaseName("users_deleted_at_idx");

        // Soft delete global query filter
        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}
