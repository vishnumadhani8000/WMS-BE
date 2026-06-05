using WMS.Application.DTOs.State;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IStateService
{
    Task<StateResponseDTO>              GetByIdAsync(int id);
    Task<PagedResult<StateResponseDTO>> GetAllAsync(CommonFilterDto requestDTO);
    Task<List<StateResponseDTO>>        GetAllStatesAsync();
    Task                                  CreateAsync(StateRequestDTO dto, int createdBy);
    Task                                   UpdateAsync(int id, StateRequestDTO dto, int updatedBy);
    Task                                    DeleteAsync(int id, int deletedBy);
}