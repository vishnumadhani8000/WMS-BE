using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Shared.Response;

namespace WMS.Application.Services;

public class OrderService : IOrderService
{
    private readonly ICommonRepository<Order> _orderRepository;
    private readonly ICommonRepository<Cart> _cartRepository;
    private readonly ICommonRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public OrderService(
        ICommonRepository<Order> orderRepository,
        ICommonRepository<Cart> cartRepository,
        ICommonRepository<Product> productRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<string> CreateOrderAsync(
        long userId,
        OrderRequestDto request)
    {
        var cart = await _cartRepository
            .Query()
            .Include(x => x.CartItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(
                x => x.Id == request.CartId
                && x.UserId == userId
                && !x.IsDeleted
                && !x.IsCheckOut);

        if (cart == null)
        {  
            throw new KeyNotFoundException("Cart not found.");

        }

        if (cart.IsCheckOut)
        {
            throw new ArgumentException("Cart already checked out.");
            
        }

        if (!cart.CartItems.Any())
        {
            throw new ArgumentException("Cart is empty.");
        }

        foreach (var item in cart.CartItems)
        {
            if (item.Product == null)
            {
                throw new KeyNotFoundException("Product not found.");

            }

            if (item.Quantity <= 0)
            {
                throw new ArgumentException($"Invalid quantity for product {item.Product.Name}.");
          
            }

            if (item.Product.Stock <= 0)
            {
                throw new ArgumentException( $"{item.Product.Name} is out of stock.");
                
            }   

            if (item.Quantity > item.Product.Stock)
            {
                throw new ArgumentException( $"Only {item.Product.Stock} quantity available for {item.Product.Name}.");
               
            }
        }

        var totalWeight = cart.CartItems.Sum(x =>
            x.Product.WeightKg * x.Quantity);

        var totalPrice = cart.CartItems.Sum(x =>
            x.Product.Price * x.Quantity);

        var status = totalWeight > 500
            ? OrderStatus.Pending
            : OrderStatus.Accepted;

        var order = new Order
        {
            UserId = userId,
            CartId = cart.Id,
            AddressId = request.AddressId,
            Notes = request.Notes,
            Status = status,
            TotalWeightKg = totalWeight,
            TotalPrice = totalPrice,

            OrderItems = cart.CartItems.Select(x =>
                new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    WeightKg = x.Product.WeightKg,
                    Price = x.Product.Price
                }).ToList()
        };

        await _orderRepository.AddAsync(order);

        foreach (var item in cart.CartItems)
        {
           item.Product.Stock -= item.Quantity;
        }

        await _productRepository.UpdateRangeAsync(
            cart.CartItems.Select(x => x.Product));
        cart.IsCheckOut = true;
        await _cartRepository.UpdateAsync(cart);

       
            return "Order created successfully.";
    }

    public async Task<OrderResponseDto> UpdateOrderAsync(
        long orderId,
        OrderUpdateDto request,
        long updatedBy)
    {
        var order = await _orderRepository
            .Query()
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == orderId);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        order.Status = request.Status;
        order.Notes = request.Notes;
        order.UpdatedAt = DateTime.UtcNow;
        order.UpdatedBy = updatedBy;
        await _orderRepository.UpdateAsync(order);
        return _mapper.Map<OrderResponseDto>(order);

       
    }

    public async Task<List<OrderResponseDto>> GetUserOrdersAsync(
    long userId)
{
    var orders = await _orderRepository
        .Query()
        .Include(x => x.Address)
            .ThenInclude(x => x.State)

        .Include(x => x.Address)
            .ThenInclude(x => x.City)

        .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)

        .Where(x => x.UserId == userId)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();

    return _mapper.Map<List<OrderResponseDto>>(orders);

 
}
}