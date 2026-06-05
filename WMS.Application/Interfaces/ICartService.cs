using WMS.Application.DTOs.Carts;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface ICartService
{
    Task<string > AddToCartAsync(
        AddToCartDto dto,
        int createdBy);

    Task<string> UpdateQuantityAsync(int cartItemId,UpdateCartItemQuantityDto dto, int updatedBy);

    Task<string> DeleteCartItemAsync(int cartItemId,int deletedBy);

    Task<CartResponseDto> GetCartAsync(int userId);
}