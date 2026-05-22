using WMS.Domain.Common;

namespace WMS.Domain.Entities;

public class Cart : BaseEntity
{
    public long UserId { get; set; }

    public decimal TotalWeightKg { get; set; }

    public User? CreatedByUser { get; set; }

    public User? UpdatedByUser { get; set; }

    public User? DeletedByUser { get; set; }

    public User User { get; set; } = null!;

    public ICollection<CartItem> CartItems { get; set; }= new List<CartItem>();
}