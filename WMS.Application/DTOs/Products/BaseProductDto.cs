namespace WMS.Application.DTOs.Products;

public class BaseProductDto
{
   public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public decimal WeightKg { get; set; }
    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string? Description { get; set; }
}