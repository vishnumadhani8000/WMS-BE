using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Carts;
using WMS.Application.Interfaces;
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
    public async Task<IActionResult> AddToCart(
        [FromBody] AddToCartDto dto,
        CancellationToken ct)
    {
        var response = await _cartService
            .AddToCartAsync(
                dto,
                UserId);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    [HttpPut("items/{cartItemId}/quantity")]
    public async Task<IActionResult> UpdateQuantity(
        long cartItemId,
        [FromBody] UpdateCartItemQuantityDto dto,
        CancellationToken ct)
    {
        var response = await _cartService
            .UpdateQuantityAsync(
                cartItemId,
                dto,
                UserId);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> DeleteCartItem(
        long cartItemId,
        CancellationToken ct)
    {
        var response = await _cartService
            .DeleteCartItemAsync(
                cartItemId,
                UserId);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetCart(
        CancellationToken ct)
    {
        var response = await _cartService
            .GetCartAsync(UserId);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}