using WMS.Application.DTOs.Vehicles;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IVehicleService
{
    Task<PagedResult<VehicleResponseDto>> GetAllAsync(CommonFilterDto requestDto);
    Task<VehicleResponseDto> GetByIdAsync(int id);
    Task<VehicleResponseDto> CreateAsync(
        VehicleRequestDto dto,
        int userId
    );

    Task<VehicleResponseDto> UpdateAsync(
        int id,
        VehicleRequestDto dto,  
        int userId
    );

    Task DeleteAsync(
        int id,
        int userId
    );
    Task<List<AvailableVehicleDto>>GetAvailableVehiclesAsync(decimal totalWeightKg);
}