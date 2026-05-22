using WMS.Application.DTOs.Carts;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface ICartService
{
    Task<ApiResponse<string>> AddToCartAsync(
        AddToCartDto dto,
        long createdBy);

    Task<ApiResponse<object>> UpdateQuantityAsync(
        long cartItemId,
        UpdateCartItemQuantityDto dto,
        long updatedBy);

    Task<ApiResponse<object>> DeleteCartItemAsync(
        long cartItemId,
        long deletedBy);

    Task<ApiResponse<CartResponseDto>> GetCartAsync(
        long userId);
}