    using WMS.Domain.Common;

    namespace WMS.Domain.Entities;

    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal WeightKg { get; set; }
        public int Stock { get; set; } = 0;
        public string? Description { get; set; }
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
        public User? DeletedByUser { get; set; }
        public ICollection<CartItem> cartItems {get;set;} = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
