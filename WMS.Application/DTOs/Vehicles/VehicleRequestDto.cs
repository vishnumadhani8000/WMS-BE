using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs.Vehicles;

public class VehicleRequestDto
{

    public string Name { get; set; } = string.Empty;

    public string PlateNumber { get; set; } = string.Empty;

    public decimal CapacityKg { get; set; }

    public bool IsAvailable { get; set; } = true;
}