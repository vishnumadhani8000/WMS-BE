public class MakeShipmentRequestDto
{
    public List<long> OrderIds { get; set; } = [];

    public long DriverId { get; set; }

    public long VehicleId { get; set; }
}   