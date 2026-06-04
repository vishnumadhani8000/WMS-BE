using WMS.Domain.Enums;

namespace WMS.Application.DTOs.Shipments;

public class UpdateShipmentStatusRequestDto
{
    public ShipmentStatus Status { get; set; }
}