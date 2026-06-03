namespace WMS.Application.DTOs.Shipments;

public class ShipmentDetailResponseDto
{
    public long ShipmentId { get; set; }

    public string DriverName { get; set; } = string.Empty;
    public string DriverPhone { get; set; } = string.Empty;

    public string VehicleNumber { get; set; } = string.Empty;
    public decimal TotalWeightKg { get; set; }
    public int TotalOrders { get; set; }
    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public List<ShipmentOrderDto> Orders { get; set; } = [];
}