using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Carts;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Shared.Response;

namespace WMS.Application.Services;

public class CartService : ICartService
{
    private readonly ICommonRepository<Cart> _cartRepository;
    private readonly ICommonRepository<CartItem> _cartItemRepository;
    private readonly ICommonRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public CartService(
        ICommonRepository<Cart> cartRepository,
        ICommonRepository<CartItem> cartItemRepository,
        ICommonRepository<Product> productRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    // ADD TO CART
    public async Task<string> AddToCartAsync(
    AddToCartDto dto,
    int createdBy)
    {
        if (dto.Quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than 0.");
        }

        var product = await _productRepository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == dto.ProductId);

        if (product == null)
        {
            throw new KeyNotFoundException(
                "Product not found.");
        }

        var cart = await _cartRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.UserId == createdBy &&
                     !x.IsCheckOut);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = createdBy,
                CreatedBy = createdBy
            };

            await _cartRepository.AddAsync(cart);
        }

        var existingCartItem = await _cartItemRepository
            .Query()
            .Include(x => x.Product)
            .FirstOrDefaultAsync(
                x => x.CartId == cart.Id &&
                     x.ProductId == dto.ProductId);

        if (existingCartItem != null)
        {
            var newQuantity =
                existingCartItem.Quantity + dto.Quantity;

            if (newQuantity > product.Stock)
            {
                throw new InvalidOperationException(
                    "All available stock is already added to your cart.");
            }

            existingCartItem.Quantity = newQuantity;
            existingCartItem.UpdatedBy = createdBy;
            existingCartItem.UpdatedAt = DateTime.UtcNow;

            await _cartItemRepository
                .UpdateAsync(existingCartItem);
        }
        else
        {
            if (dto.Quantity > product.Stock)
            {
                throw new InvalidOperationException(
                    $"Only {product.Stock} quantity available.");
            }

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                CreatedBy = createdBy
            };

            await _cartItemRepository
                .AddAsync(cartItem);
        }

        return "Product added to cart.";
    }
    // UPDATE QUANTITY
    public async Task<string> UpdateQuantityAsync(
        int cartItemId,
        UpdateCartItemQuantityDto dto,
        int updatedBy)
    {
        if (dto.Quantity <= 0)
        {
            throw new ArgumentException(
            "Quantity must be greater than 0.");
        }

        var cartItem = await _cartItemRepository
            .Query()
            .Include(x => x.Product)
            .FirstOrDefaultAsync(
                x => x.Id == cartItemId);

        if (cartItem == null)
        {
            throw new KeyNotFoundException(
           "Cart item not found.");
        }

        if (dto.Quantity > cartItem.Product.Stock)
        {
            throw new ArgumentException(
            $"Only {cartItem.Product.Stock} quantity available.");
        }

        cartItem.Quantity = dto.Quantity;
        cartItem.UpdatedBy = updatedBy;
        cartItem.UpdatedAt = DateTime.UtcNow;

        await _cartItemRepository
            .UpdateAsync(cartItem);

        return "Cart item updated successfully.";
    }

    // DELETE CART ITEM
    public async Task<string> DeleteCartItemAsync(
        int cartItemId,
        int deletedBy)
    {
        var cartItem = await _cartItemRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == cartItemId &&
                     x.DeletedAt == null);

        if (cartItem == null)
        {
            throw new KeyNotFoundException(
          "Cart item not found.");
        }

        cartItem.DeletedBy = deletedBy;

        await _cartItemRepository
            .SoftDeleteAsync(cartItem);

        return "Cart item deleted successfully.";
    }

    // GET CART
    public async Task<CartResponseDto> GetCartAsync(int userId)
    {
        var cart = await _cartRepository
       .Query()
       .Include(x => x.CartItems)
       .ThenInclude(x => x.Product)
       .FirstOrDefaultAsync(
           x => x.UserId == userId &&
                !x.IsCheckOut);

        if (cart == null)
        {
            if (cart == null)
            {
                return new CartResponseDto();
            }
        }

        bool updated = false;

        foreach (var item in cart.CartItems.ToList())
        {
            if (item.Product == null)
            {
                item.DeletedAt = DateTime.UtcNow;

                await _cartItemRepository
                    .SoftDeleteAsync(item);

                updated = true;

                continue;
            }

            if (item.Product.Stock <= 0)
            {
                item.DeletedAt = DateTime.UtcNow;

                await _cartItemRepository
                    .SoftDeleteAsync(item);

                updated = true;

                continue;
            }

            if (item.Quantity > item.Product.Stock)
            {
                item.Quantity = item.Product.Stock;
                item.UpdatedAt = DateTime.UtcNow;

                await _cartItemRepository
                    .UpdateAsync(item);

                updated = true;
            }
        }

        if (updated)
        {
            cart = await _cartRepository
                .Query()
                .AsNoTracking()
                .Include(x => x.CartItems
                    .Where(ci => ci.DeletedAt == null))
                .ThenInclude(x => x.Product)
                .FirstAsync(x => x.Id == cart.Id);
        }

        var response = _mapper.Map<CartResponseDto>(cart);

        response.TotalWeightKg = cart.CartItems
            .Where(x => x.DeletedAt == null && x.Product != null)
            .Sum(x => x.Product.WeightKg * x.Quantity);

        return response;
    }
}