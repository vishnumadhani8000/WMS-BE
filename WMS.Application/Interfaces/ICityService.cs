using WMS.Application.DTOs.State;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface ICityService
{
    Task<CityResponseDTO>             GetByIdAsync(int id);
    Task<PagedResult<CityResponseDTO>> GetAllAsync(CityFilterRequestDTO requestDTO);
    Task                               CreateAsync(CityRequestDTO dto, int createdBy);
    Task                               UpdateAsync(int id, CityRequestDTO dto, int updatedBy);
    Task                               DeleteAsync(int id, int deletedBy);
    Task<List<CityResponseDTO>> GetCitiesByStateAsync(int stateId);
}