using WMS.Domain.Common;

namespace WMS.Domain.Entities;

public class Driver : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? LicenceNo { get; set; }
    public bool IsAvailable { get; set; } = true;

    public User? CreatedByUser { get; set; }
    public User? UpdatedByUser { get; set; }
    public User? DeletedByUser { get; set; }


    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
