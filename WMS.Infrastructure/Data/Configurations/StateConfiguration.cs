using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data.Configurations;

public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.ToTable("states");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.Name)      .IsRequired().HasMaxLength(100);
        builder.Property(x => x.CreatedAt) .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.UpdatedAt) .IsRequired().HasDefaultValueSql("now()");
        builder.Property(x => x.DeletedAt) .IsRequired(false);
        builder.Property(x => x.CreatedBy) .IsRequired(false);
        builder.Property(x => x.UpdatedBy) .IsRequired(false);
        builder.Property(x => x.DeletedBy) .IsRequired(false);

        builder.HasIndex(x => x.Name)      .IsUnique().HasDatabaseName("states_name_uidx");
        builder.HasIndex(x => x.DeletedAt) .HasDatabaseName("states_deleted_at_idx");

        builder.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.UpdatedByUser).WithMany().HasForeignKey(x => x.UpdatedBy).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.DeletedByUser).WithMany().HasForeignKey(x => x.DeletedBy).OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}
