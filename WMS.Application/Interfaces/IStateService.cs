using WMS.Application.DTOs.State;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IStateService
{
    Task<ApiResponse<StateResponseDTO>>              GetByIdAsync(long id);
    Task<ApiResponse<PagedResult<StateResponseDTO>>> GetAllAsync(CommonFilterDto requestDTO);
    Task<ApiResponse<bool>>                          CreateAsync(StateRequestDTO dto, long createdBy);
    Task<ApiResponse<bool>>                          UpdateAsync(long id, StateRequestDTO dto, long updatedBy);
    Task<ApiResponse<bool>>                          DeleteAsync(long id, long deletedBy);
}