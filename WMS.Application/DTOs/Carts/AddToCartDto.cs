namespace WMS.Application.DTOs.Carts;

public class AddToCartDto
{
    public long ProductId { get; set; }

    public int Quantity { get; set; }
}