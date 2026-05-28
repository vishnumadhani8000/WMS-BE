public class OrderItemResponseDto
{
    public long OrderItemId { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal WeightKg { get; set; }
    public decimal Price { get; set; }
}