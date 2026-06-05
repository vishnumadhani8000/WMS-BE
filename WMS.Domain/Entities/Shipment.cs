using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities;

public class Shipment : BaseEntity
{
    public int DriverId { get; set; }
    public int VehicleId { get; set; }
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Assigned;
    public decimal TotalWeightKg { get; set; }
    public DateTime? DispatchedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public User? CreatedByUser { get; set; }
    public User? UpdatedByUser { get; set; }
    public User? DeletedByUser { get; set; }
    public Driver Driver { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
