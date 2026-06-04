using WMS.Application.DTOs.Carts;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface ICartService
{
    Task<string > AddToCartAsync(
        AddToCartDto dto,
        long createdBy);

    Task<string> UpdateQuantityAsync(long cartItemId,UpdateCartItemQuantityDto dto, long updatedBy);

    Task<string> DeleteCartItemAsync(long cartItemId,long deletedBy);

    Task<CartResponseDto> GetCartAsync(long userId);
}