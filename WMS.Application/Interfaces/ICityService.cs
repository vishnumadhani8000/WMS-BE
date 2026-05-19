using WMS.Application.DTOs.State;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface ICityService
{
    Task<ApiResponse<CityResponseDTO>>              GetByIdAsync(long id);
    Task<ApiResponse<PagedResult<CityResponseDTO>>> GetAllAsync(CityFilterRequestDTO requestDTO);
    Task<ApiResponse<bool>>                          CreateAsync(CityRequestDTO dto, long createdBy);
    Task<ApiResponse<bool>>                          UpdateAsync(long id, CityRequestDTO dto, long updatedBy);
    Task<ApiResponse<bool>>                          DeleteAsync(long id, long deletedBy);
}