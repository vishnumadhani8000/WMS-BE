using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IOrderService
{
    Task<ApiResponse<string >> CreateOrderAsync(
        long userId,
        OrderRequestDto request);

    Task<ApiResponse<OrderResponseDto>> UpdateOrderAsync(
        long orderId,
        OrderUpdateDto request,
        long updatedBy);

    Task<ApiResponse<List<OrderResponseDto>>> GetUserOrdersAsync(
        long userId);
}