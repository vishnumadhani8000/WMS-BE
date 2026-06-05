using WMS.Application.Common;
using WMS.Application.DTOs.UserAddresses;
using WMS.Shared.Response;

namespace WMS.Application.Services;

public interface IUserAddressService
{
    Task<IEnumerable<UserAddressResponseDto>> GetUserAddressesAsync(
        int userId);

    Task<UserAddressResponseDto> GetByIdAsync(
        int addressId,
        int userId);

    Task<UserAddressResponseDto> CreateAsync(
        int userId,
        UserAddressRequestDto dto);

    Task UpdateAsync(
        int addressId,
        int userId,
        UserAddressRequestDto dto);

    Task DeleteAsync(
        int addressId,
        int userId);
}