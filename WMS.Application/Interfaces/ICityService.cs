using WMS.Application.DTOs.State;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface ICityService
{
    Task<CityResponseDTO>             GetByIdAsync(long id);
    Task<PagedResult<CityResponseDTO>> GetAllAsync(CityFilterRequestDTO requestDTO);
    Task                               CreateAsync(CityRequestDTO dto, long createdBy);
    Task                               UpdateAsync(long id, CityRequestDTO dto, long updatedBy);
    Task                               DeleteAsync(long id, long deletedBy);
    Task<List<CityResponseDTO>> GetCitiesByStateAsync(long stateId);
}