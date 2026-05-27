using WMS.Domain.Enums;

public class OrderUpdateDto
{
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }
}