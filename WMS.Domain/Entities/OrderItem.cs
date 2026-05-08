using WMS.Domain.Common;

namespace WMS.Domain.Entities;

public class OrderItem : BaseEntity
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitWeightKg { get; set; }
    public decimal LineWeightKg { get; set; }

    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
