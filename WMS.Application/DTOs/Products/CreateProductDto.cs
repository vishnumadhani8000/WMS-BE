namespace WMS.Application.DTOs.Products;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;

    public decimal WeightKg { get; set; }

    public int Stock { get; set; }

    public string? Description { get; set; }
}