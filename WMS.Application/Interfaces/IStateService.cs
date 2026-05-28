using WMS.Application.DTOs.State;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IStateService
{
    Task<StateResponseDTO>              GetByIdAsync(long id);
    Task<PagedResult<StateResponseDTO>> GetAllAsync(CommonFilterDto requestDTO);
    Task<List<StateResponseDTO>>        GetAllStatesAsync();
    Task                                  CreateAsync(StateRequestDTO dto, long createdBy);
    Task                                   UpdateAsync(long id, StateRequestDTO dto, long updatedBy);
    Task                                    DeleteAsync(long id, long deletedBy);
}