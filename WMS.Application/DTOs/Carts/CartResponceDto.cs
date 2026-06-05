namespace WMS.Application.DTOs.Carts;

public class CartResponseDto
{
    public int CartId { get; set; }

    public int UserId { get; set; }

    public decimal TotalWeightKg { get; set; }

    public List<CartItemResponseDto> Items { get; set; } = [];
}