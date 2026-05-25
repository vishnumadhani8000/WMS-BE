    using WMS.Domain.Common;
    using WMS.Domain.Enums;

    namespace WMS.Domain.Entities;

    public class Order : BaseEntity
    {
        public long UserId { get; set; }

        public long CartId { get; set; }
        public long AddressId { get; set; }
        public long? ShipmentId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalWeightKg { get; set; }
        public decimal TotalPrice {get;set;}
        public string? Notes { get; set; }
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
        public User? DeletedByUser { get; set; }
        public Cart Cart {get;set;} 

        public User User { get; set; } = null!;
        public UserAddress Address { get; set; } = null!;
        public Shipment? Shipment { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
