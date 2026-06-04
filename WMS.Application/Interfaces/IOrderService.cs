using WMS.Application.DTOs.Order;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IOrderService
{
    Task<string> CreateOrderAsync(
        long userId,
        OrderRequestDto request);

    Task<OrderResponseDto> UpdateOrderAsync(
        long orderId,
        OrderUpdateDto request,
        long updatedBy);

    Task<List<OrderResponseDto>> GetUserOrdersAsync(
        long userId);

    Task<PagedResult<AdminOrderResponseDto>> GetAllOrdersAsync(AdminOrderRequestDto requestDto);
    Task<AdminOrderDetailResponseDto> GetOrderByIdAsync(long id);
}