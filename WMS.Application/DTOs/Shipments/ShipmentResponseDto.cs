namespace WMS.Application.DTOs.Shipments;

public class ShipmentResponseDto
{
    public int ShipmentId { get; set; }

    public string DriverName { get; set; } = string.Empty;

    public string VehicleNumber { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}