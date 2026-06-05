using WMS.Application.DTOs.Drivers;
using WMS.Domain.Common;

namespace WMS.Application.Interfaces;

public interface IDriverService
{
    Task<PagedResult<DriverResponseDto>> GetAllAsync(
        CommonFilterDto requestDto);

    Task<DriverResponseDto> GetByIdAsync(
        int id);

    Task<DriverResponseDto> CreateAsync(
        DriverRequestDto dto,
        int userId);

    Task<DriverResponseDto> UpdateAsync(
        int id,
        DriverRequestDto dto,
        int userId);

    Task DeleteAsync(
        int id,
        int userId);

    Task<List<AvailableDriversDto>> GetAvailableDriversAsync();
}