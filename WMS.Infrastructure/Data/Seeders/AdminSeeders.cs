using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Infrastructure.Data.Seeders;

public static class AdminSeeder
{
    public static async Task SeedAsync(WmsDbContext db)
    {
        if (db.Users.Any(x => x.Role == UserRole.Admin)) return;

        db.Users.Add(new User
        {
            Name         = "WMS Admin",
            Email        = "admin@wms.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role         = UserRole.Admin,
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow,
            UpdatedAt    = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }
}