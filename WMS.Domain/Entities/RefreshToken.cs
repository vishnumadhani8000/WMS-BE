using WMS.Domain.Common;

namespace WMS.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiryDate { get; set; }

    public bool IsRevoked { get; set; } = false;

    public long UserId { get; set; }

    public User User { get; set; } = null!;
}