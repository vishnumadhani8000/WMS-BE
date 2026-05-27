using WMS.Application.DTOs.UserAddresses;
using WMS.Domain.Enums;

public class OrderResponseDto
{
    public long OrderId { get; set; }


    public decimal TotalPrice { get; set; }

    public string? Notes { get; set; }

    public OrderStatus Status { get; set; }

    public UserAddressResponseDto Address { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public List<OrderItemResponseDto> Items { get; set; } = [];
}

