using WMS.Application.Common;
using WMS.Application.DTOs.UserAddresses;
using WMS.Shared.Response;

namespace WMS.Application.Services;

public interface IUserAddressService
{
    Task<ApiResponse<IEnumerable<UserAddressResponseDto>>> GetUserAddressesAsync(
        long userId);

    Task<ApiResponse<UserAddressResponseDto>> GetByIdAsync(
        long addressId,
        long userId);

    Task<ApiResponse<UserAddressResponseDto>> CreateAsync(
        long userId,
        UserAddressRequestDto dto);

    Task<ApiResponse<string>> UpdateAsync(
        long addressId,
        long userId,
        UserAddressRequestDto dto);

    Task<ApiResponse<string>> DeleteAsync(
        long addressId,
        long userId);
}