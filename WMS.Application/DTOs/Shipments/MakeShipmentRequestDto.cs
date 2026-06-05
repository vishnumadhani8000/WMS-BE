public class MakeShipmentRequestDto
{
    public List<int> OrderIds { get; set; } = [];

    public int DriverId { get; set; }

    public int VehicleId { get; set; }
}   