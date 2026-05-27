using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CreateOrder(
    [FromBody] OrderRequestDto request)
    {
        var userId = Convert.ToInt64(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        var response = await _orderService
            .CreateOrderAsync(userId, request);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrder(
        long orderId,
        [FromBody] OrderUpdateDto request)
    {
        var userId = Convert.ToInt64(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        var response = await _orderService
            .UpdateOrderAsync(orderId, request, userId);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserOrders()
    {
        var userId = Convert.ToInt64(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        var response = await _orderService
            .GetUserOrdersAsync(userId);

        return Ok(response);
    }
}