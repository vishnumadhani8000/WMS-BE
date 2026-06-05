namespace WMS.Application.DTOs.Products;

public class ProductResponseCustomerDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public string? Description { get; set; }
}