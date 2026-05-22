namespace WMS.Application.DTOs.Carts;

public class CartItemResponseDto
{
    public long CartItemId { get; set; }

    public long ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public int AvailableStock { get; set; }

    public decimal WeightKg { get; set; }
}