namespace WMS.Application.DTOs.Vehicles;

public class VehicleResponseDto
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string PlateNumber { get; set; } = string.Empty;

    public decimal CapacityKg { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }
}