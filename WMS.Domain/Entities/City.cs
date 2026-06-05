using WMS.Domain.Common;
using WMS.Domain.Entities;

namespace WMS.Domain.Entities;

public class City : BaseEntity
{
    public int StateId { get; set; }
    public string Name { get; set; } = string.Empty;

    public User? CreatedByUser { get; set; }
    public User? UpdatedByUser { get; set; }
    public User? DeletedByUser { get; set; }

    public State State { get; set; } = null!;
    public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
