
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS.Application.DTOs.Order;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
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

    private long UserId => Convert.ToInt64(User.FindFirstValue(ClaimTypes.NameIdentifier));

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

    [HttpGet("{my-orders}")]
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
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var order = await _orderService
            .GetOrderByIdAsync(id);

        return Ok(
            ApiResponse<AdminOrderDetailResponseDto>
                .Success(order));
    }


    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] AdminOrderRequestDto request)
    {
        var orders = await _orderService
            .GetAllOrdersAsync(request);

        return Ok(
            ApiResponse<PagedResult<AdminOrderResponseDto>>
                .Success(orders));
    }

}

