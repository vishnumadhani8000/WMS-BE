namespace WMS.Application.DTOs.Shipments;

public class ShipmentOrderDto
{
    public long OrderId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public decimal TotalWeightKg { get; set; }
    public long totalPrice { get; set; }

    public string CityName { get; set; } = string.Empty;

    public string StateName { get; set; } = string.Empty;


}