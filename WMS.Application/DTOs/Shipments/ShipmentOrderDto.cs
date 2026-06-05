namespace WMS.Application.DTOs.Shipments;

public class ShipmentOrderDto
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public decimal TotalWeightKg { get; set; }
    public int totalPrice { get; set; }

    public string CityName { get; set; } = string.Empty;

    public string StateName { get; set; } = string.Empty;


}