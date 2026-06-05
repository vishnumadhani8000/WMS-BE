using WMS.Application.DTOs.Order;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IOrderService
{
    Task<string> CreateOrderAsync(
        int userId,
        OrderRequestDto request);

    Task<OrderResponseDto> UpdateOrderAsync(
        int orderId,
        OrderUpdateDto request,
        int updatedBy);

    Task<List<OrderResponseDto>> GetUserOrdersAsync(
        int userId);

    Task<PagedResult<AdminOrderResponseDto>> GetAllOrdersAsync(AdminOrderRequestDto requestDto);
    Task<AdminOrderDetailResponseDto> GetOrderByIdAsync(int id);
}