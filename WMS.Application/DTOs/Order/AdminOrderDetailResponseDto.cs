using WMS.Domain.Enums;

public class AdminOrderDetailResponseDto
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string AddressLine { get; set; } = string.Empty;

    public string? Landmark { get; set; }

    public string CityName { get; set; } = string.Empty;

    public string StateName { get; set; } = string.Empty;

    public string? Pincode { get; set; } 

    public decimal TotalPrice { get; set; }

    public decimal TotalWeightKg { get; set; }

    public OrderStatus Status { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<OrderItemResponseDto> Items { get; set; }
        = new();
}