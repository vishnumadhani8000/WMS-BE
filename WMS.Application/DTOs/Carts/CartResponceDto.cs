namespace WMS.Application.DTOs.Carts;

public class CartResponseDto
{
    public long CartId { get; set; }

    public long UserId { get; set; }

    public decimal TotalWeightKg { get; set; }

    public List<CartItemResponseDto> Items { get; set; } = [];
}