using WMS.Application.DTOs.Vehicles;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IVehicleService
{
    Task<ApiResponse<PagedResult<VehicleResponseDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? sortBy,
        bool ascending
    );

    Task<ApiResponse<VehicleResponseDto>> GetByIdAsync(long id);

    Task<ApiResponse<VehicleResponseDto>> CreateAsync(
        VehicleRequestDto dto,
        long userId
    );

    Task<ApiResponse<object>> UpdateAsync(
        long id,
        VehicleRequestDto dto,
        long userId
    );

    Task<ApiResponse<object>> DeleteAsync(
        long id,
        long userId
    );
}