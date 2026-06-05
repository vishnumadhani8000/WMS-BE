using WMS.Domain.Common;

namespace WMS.Domain.Entities;

public class UserAddress : BaseEntity
{
    public int UserId { get; set; }
    public int StateId { get; set; }
    public int CityId { get; set; }
    public string AddressLine { get; set; } = string.Empty;
    public string? Landmark { get; set; }
    public string Pincode { get; set; } = string.Empty;
    public bool IsDefault { get; set; } = false;

    public User User { get; set; } = null!;
    public State State { get; set; } = null!;
    public City City { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
