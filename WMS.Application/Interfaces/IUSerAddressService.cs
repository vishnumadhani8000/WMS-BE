using WMS.Application.Common;
using WMS.Application.DTOs.UserAddresses;
using WMS.Shared.Response;

namespace WMS.Application.Services;

public interface IUserAddressService
{
    Task<IEnumerable<UserAddressResponseDto>> GetUserAddressesAsync(
        long userId);

    Task<UserAddressResponseDto> GetByIdAsync(
        long addressId,
        long userId);

    Task<UserAddressResponseDto> CreateAsync(
        long userId,
        UserAddressRequestDto dto);

    Task UpdateAsync(
        long addressId,
        long userId,
        UserAddressRequestDto dto);

    Task DeleteAsync(
        long addressId,
        long userId);
}