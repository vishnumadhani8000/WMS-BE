
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS.Application.Interfaces;
using WMS.Shared.Response;

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

    private long UserId =>Convert.ToInt64( User.FindFirstValue( ClaimTypes.NameIdentifier));

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] OrderRequestDto request)
    {
        var message = await _orderService
            .CreateOrderAsync(
                UserId,
                request);

        return Ok(
            ApiResponse<string>.Success(
                message
            )
        );
    }

    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrder(
        long orderId,
        [FromBody] OrderUpdateDto request)
    {
        var order = await _orderService
            .UpdateOrderAsync(
                orderId,
                request,
                UserId);

        return Ok(
            ApiResponse<OrderResponseDto>.Success(
                order,
                "Order updated successfully."
            )
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetUserOrders()
    {
        var orders = await _orderService
            .GetUserOrdersAsync(
                UserId);

        return Ok(
            ApiResponse<List<OrderResponseDto>>.Success(
                orders,
                "Orders fetched successfully."
            )
        );
    }
}

