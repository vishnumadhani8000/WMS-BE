using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities;

public class Shipment : BaseEntity
{
    public long DriverId { get; set; }
    public long VehicleId { get; set; }
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Draft;
    public decimal TotalWeightKg { get; set; }
    public DateTime? DispatchedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string? Notes { get; set; }
    public User? CreatedByUser { get; set; }
    public User? UpdatedByUser { get; set; }
    public User? DeletedByUser { get; set; }
    public Driver Driver { get; set; } = null!;
    public Vehicle Vehicle { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
