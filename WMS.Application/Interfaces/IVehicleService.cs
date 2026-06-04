using WMS.Application.DTOs.Vehicles;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IVehicleService
{
    Task<PagedResult<VehicleResponseDto>> GetAllAsync(CommonFilterDto requestDto);
    Task<VehicleResponseDto> GetByIdAsync(long id);
    Task<VehicleResponseDto> CreateAsync(
        VehicleRequestDto dto,
        long userId
    );

    Task<VehicleResponseDto> UpdateAsync(
        long id,
        VehicleRequestDto dto,  
        long userId
    );

    Task DeleteAsync(
        long id,
        long userId
    );
    Task<List<AvailableVehicleDto>>GetAvailableVehiclesAsync(decimal totalWeightKg);
}