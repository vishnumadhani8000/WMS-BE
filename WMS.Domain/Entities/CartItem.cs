using WMS.Domain.Common;

namespace WMS.Domain.Entities;

public class CartItem : BaseEntity
{
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public Cart Cart { get; set; } = null!;

    public Product Product { get; set; } = null!;
}

