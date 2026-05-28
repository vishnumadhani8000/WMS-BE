using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Carts;
using WMS.Application.Interfaces;
using WMS.Shared.Response;
namespace WMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(
        ICartService cartService)
    {
        _cartService = cartService;
    }

    private long UserId =>
        long.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );


    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto, CancellationToken ct)
    {
        var message = await _cartService
            .AddToCartAsync(dto, UserId);

        return Ok(ApiResponse<string>.Success(message));
    }


    [HttpPut("items/{cartItemId}/quantity")]
    public async Task<IActionResult> UpdateQuantity(long cartItemId, [FromBody] UpdateCartItemQuantityDto dto, CancellationToken ct)
    {
        var message = await _cartService
            .UpdateQuantityAsync(
                cartItemId,
                dto,
                UserId);

        return Ok(
            ApiResponse<string>.Success(message)
        );
    }
    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> DeleteCartItem(
     long cartItemId,
     CancellationToken ct)
    {
        var message = await _cartService
            .DeleteCartItemAsync(
                cartItemId,
                UserId);

        return Ok(
            ApiResponse<string>.Success(message)
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetCart(
    CancellationToken ct)
    {
        var cart = await _cartService
            .GetCartAsync(UserId);

        return Ok(
            ApiResponse<CartResponseDto>.Success(cart)
        );
    }
}