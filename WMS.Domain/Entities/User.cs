    using WMS.Domain.Common;
    using WMS.Domain.Enums;

    namespace WMS.Domain.Entities;

    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Customer;
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
