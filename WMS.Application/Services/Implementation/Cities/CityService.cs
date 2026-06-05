using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.State;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Domain.Entities;
using WMS.Shared.Response;

namespace WMS.Application.Services;

public class CityService : ICityService
{
    private readonly ICommonRepository<City> _repository;
    private readonly ICommonRepository<State> _stateRepository;
    private readonly IMapper _mapper;

    public CityService(
        ICommonRepository<City> repository,
        ICommonRepository<State> stateRepository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _stateRepository = stateRepository;
    }

    public async Task<PagedResult<CityResponseDTO>> GetAllAsync(CityFilterRequestDTO requestDTO)
    {
        IQueryable<City> query = _repository
            .Query().
            Where(x => x.StateId == requestDTO.stateId);

        if (!string.IsNullOrWhiteSpace(requestDTO.Search))
        {
            requestDTO.Search = requestDTO.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.ToLower().Contains(requestDTO.Search)
            );

        }
        var stateExists = await _stateRepository
            .ExistsAsync(x => x.Id == requestDTO.stateId);
        if (!stateExists)
        {
            throw new KeyNotFoundException("State not found.");
        }
        query = requestDTO.SortBy?.ToLower() switch
        {
            "name" => requestDTO.Ascending
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var cities = await query
            .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
            .Take(requestDTO.PageSize)
            .ToListAsync();

        var result = new PagedResult<CityResponseDTO>
        {
            Items = _mapper.Map<List<CityResponseDTO>>(cities),
            TotalCount = totalCount,
            PageNumber = requestDTO.PageNumber,
            PageSize = requestDTO.PageSize,
        };

        return result;
    }

    public async Task<CityResponseDTO> GetByIdAsync(int id)
    {
        var city = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (city == null)
            throw new KeyNotFoundException("City not found");

        return _mapper.Map<CityResponseDTO>(city);


    }

    public async Task CreateAsync(
        CityRequestDTO dto,
        int createdBy)
    {
        dto.Name = dto.Name.Trim();

        var stateExists = await _stateRepository
            .ExistsAsync(x => x.Id == dto.StateId);

        if (!stateExists)
            throw new KeyNotFoundException("State not found");

        var exists = await _repository.ExistsAsync(
            x => x.Name.ToLower() == dto.Name.ToLower());

        if (exists)
            throw new ArgumentException(
                "City with this name already exists.");

        var city = _mapper.Map<City>(dto);
        city.CreatedBy = createdBy;

        await _repository.AddAsync(city);
    }

    public async Task UpdateAsync(int id, CityRequestDTO dto, int updatedBy)
    {
        dto.Name = dto.Name.Trim();

        var city = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (city == null)
            throw new KeyNotFoundException("City not found.");

        var duplicate = await _repository
            .Query()
            .AnyAsync(x =>
                x.Id != id &&
                x.Name.ToLower() == dto.Name.ToLower());

        if (duplicate)
            throw new ArgumentException(
                "Another city with this name already exists.");

        city.Name = dto.Name;
        city.UpdatedBy = updatedBy;
        city.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(city);
    }
    public async Task DeleteAsync(int id, int deletedBy)
    {
        var city = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (city == null)
            throw new KeyNotFoundException("City not found.");

        city.DeletedBy = deletedBy;
        city.DeletedAt = DateTime.UtcNow;

        await _repository.SoftDeleteAsync(city);
    }
    public async Task<List<CityResponseDTO>> GetCitiesByStateAsync(int stateId)
    {
        var stateExists = await _stateRepository
            .ExistsAsync(x => x.Id == stateId);

        if (!stateExists)
            throw new KeyNotFoundException("State not found.");

        var cities = await _repository
            .Query()
            .Where(x => x.StateId == stateId)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return _mapper.Map<List<CityResponseDTO>>(cities);
    }

}