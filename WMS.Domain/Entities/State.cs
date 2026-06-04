using WMS.Domain.Common;

namespace WMS.Domain.Entities;

public class State : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public User? CreatedByUser { get; set; }
    public User? UpdatedByUser { get; set; }
    public User? DeletedByUser { get; set; }

    public ICollection<City> Cities { get; set; } = new List<City>();
    public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
}
