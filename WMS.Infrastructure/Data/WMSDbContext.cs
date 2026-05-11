using Microsoft.EntityFrameworkCore;
using WMS.Domain.Common;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data;

public class WmsDbContext : DbContext
{
    public WmsDbContext(DbContextOptions<WmsDbContext> options) : base(options) { }

    // ── DbSets
    public DbSet<User>        Users        => Set<User>();
    public DbSet<State>       States       => Set<State>();
    public DbSet<City>        Cities       => Set<City>();
    public DbSet<UserAddress> UserAddresses => Set<UserAddress>();
    public DbSet<Product>     Products     => Set<Product>();
    public DbSet<Order>       Orders       => Set<Order>();
    public DbSet<OrderItem>   OrderItems   => Set<OrderItem>();
    public DbSet<Driver>      Drivers      => Set<Driver>();
    public DbSet<Vehicle>     Vehicles     => Set<Vehicle>();
    public DbSet<Shipment>    Shipments    => Set<Shipment>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WmsDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        SetAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;
        }
    }
}
