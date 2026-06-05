
namespace WMS.Application.DTOs.Order;
public class AdminOrderResponseDto
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; }

    public string CityName { get; set; }
    public string StateName { get; set; }
    public decimal TotalWeightKg {get;set;}
    public int TotalItems { get; set; }

    public decimal TotalPrice{ get; set; }

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }
}