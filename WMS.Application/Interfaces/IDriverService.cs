using WMS.Application.DTOs.Drivers;
using WMS.Domain.Common;

namespace WMS.Application.Interfaces;

public interface IDriverService
{
    Task<PagedResult<DriverResponseDto>> GetAllAsync(
        CommonFilterDto requestDto);

    Task<DriverResponseDto> GetByIdAsync(
        long id);

    Task<DriverResponseDto> CreateAsync(
        DriverRequestDto dto,
        long userId);

    Task<DriverResponseDto> UpdateAsync(
        long id,
        DriverRequestDto dto,
        long userId);

    Task DeleteAsync(
        long id,
        long userId);
}