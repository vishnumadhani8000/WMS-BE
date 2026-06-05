using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Order;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
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
        int userId,
        OrderRequestDto request)
    {
        var cart = await _cartRepository
            .Query()
            .Include(x => x.CartItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(
                x => x.Id == request.CartId
                && x.UserId == userId
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
                throw new ArgumentException($"{item.Product.Name} is out of stock.");

            }

            if (item.Quantity > item.Product.Stock)
            {
                throw new ArgumentException($"Only {item.Product.Stock} quantity available for {item.Product.Name}.");

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
        int orderId,
        OrderUpdateDto request,
        int updatedBy)
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
    int userId)
    {
        var orders = await _orderRepository
            .Query()
            .IgnoreQueryFilters()
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

    public async Task<PagedResult<AdminOrderResponseDto>> GetAllOrdersAsync(AdminOrderRequestDto request)
    {
        IQueryable<Order> query = _orderRepository
            .Query()
            .IgnoreQueryFilters()
            .Include(x => x.User)
            .Include(x => x.Address)
                .ThenInclude(x => x.State)
            .Include(x => x.Address)
                .ThenInclude(x => x.City)
            .Include(x => x.OrderItems);

        if (request.OnlyPending == true)
        {
            query = query.Where(x =>
                x.Status == OrderStatus.Pending);
        }
        if (request.onlypendingandaccepted == true)
        {
            query = query.Where(x =>
                x.Status == OrderStatus.Pending ||
                x.Status == OrderStatus.Accepted);
        }
        if (request.StateId.HasValue)
        {
            query = query.Where(x =>
                x.Address.StateId == request.StateId.Value);
        }

        if (request.CityId.HasValue)
        {
            query = query.Where(x =>
                x.Address.CityId == request.CityId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            request.Search = request.Search
                .Trim()
                .ToLower();

            query = query.Where(x =>
                x.User.Name.ToLower().Contains(request.Search)
                ||
                x.Id.ToString().Contains(request.Search));
        }

        query = request.SortBy?.ToLower() switch
        {

            "orderid" => request.Ascending
               ? query.OrderBy(x => x.Id)
               : query.OrderByDescending(x => x.Id),

            "customername" => request.Ascending
                ? query.OrderBy(x => x.User.Name)
                : query.OrderByDescending(x => x.User.Name),

            "createdat" => request.Ascending
                ? query.OrderBy(x => x.CreatedAt)
                : query.OrderByDescending(x => x.CreatedAt),

            "totalprice" => request.Ascending
                ? query.OrderBy(x => x.TotalPrice)
                : query.OrderByDescending(x => x.TotalPrice),

            "totalitems" => request.Ascending
                ? query.OrderBy(x => x.OrderItems.Sum(i => i.Quantity))
                : query.OrderByDescending(x => x.OrderItems.Sum(i => i.Quantity)),

            "totalweightkg" => request.Ascending
                ? query.OrderBy(x => x.TotalWeightKg)
                : query.OrderByDescending(x => x.TotalWeightKg),

            "cityname" => request.Ascending
                ? query.OrderBy(x => x.Address.City)
                : query.OrderByDescending(x => x.Address.City),

            "statename" => request.Ascending
                ? query.OrderBy(x => x.Address.State)
                : query.OrderByDescending(x => x.Address.State),


            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var orders = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResult<AdminOrderResponseDto>
        {
            Items = _mapper.Map<List<AdminOrderResponseDto>>(orders),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<AdminOrderDetailResponseDto>
        GetOrderByIdAsync(int orderId)
    {
        var order = await _orderRepository
            .Query()
            .IgnoreQueryFilters()
            .Include(x => x.User)

            .Include(x => x.Address)
                .ThenInclude(x => x.State)

            .Include(x => x.Address)
                .ThenInclude(x => x.City)

            .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)

            .FirstOrDefaultAsync(x =>
                x.Id == orderId);

        if (order == null)
        {
            throw new KeyNotFoundException(
                "Order not found.");
        }

        return _mapper.Map<AdminOrderDetailResponseDto>(
            order);
    }

}