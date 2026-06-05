using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.State;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Domain.Entities;
using WMS.Shared.Response;

namespace WMS.Application.Services;

public class StateService : IStateService
{
    private readonly ICommonRepository<State> _repository;
    private readonly ICommonRepository<City> _cityrepository;
    private readonly IMapper _mapper;

    public StateService(
        ICommonRepository<State> repository,
        IMapper mapper,
        ICommonRepository<City> cityrepository)
    {
        _repository = repository;
        _mapper = mapper;
        _cityrepository = cityrepository;
    }

    public async Task<PagedResult<StateResponseDTO>> GetAllAsync(
        CommonFilterDto requestDTO)
    {
        IQueryable<State> query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(requestDTO.Search))
        {
            requestDTO.Search = requestDTO.Search.Trim().ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(requestDTO.Search));
        }

        query = requestDTO.SortBy?.ToLower() switch
        {
            "name" => requestDTO.Ascending
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var states = await query
            .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
            .Take(requestDTO.PageSize)
            .ToListAsync();

        return new PagedResult<StateResponseDTO>
        {
            Items = _mapper.Map<List<StateResponseDTO>>(states),
            TotalCount = totalCount,
            PageNumber = requestDTO.PageNumber,
            PageSize = requestDTO.PageSize
        };
    }

    public async Task<StateResponseDTO> GetByIdAsync(int id)
    {
        var state = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (state == null)
            throw new KeyNotFoundException("State not found.");

        return _mapper.Map<StateResponseDTO>(state);
    }

    public async Task CreateAsync(
        StateRequestDTO dto,
        int createdBy)
    {
        dto.Name = dto.Name.Trim();

        var exists = await _repository.ExistsAsync(
            x => x.Name.ToLower() == dto.Name.ToLower());

        if (exists)
            throw new ArgumentException(
                "State with this name already exists.");

        var state = _mapper.Map<State>(dto);
        state.CreatedBy = createdBy;

        await _repository.AddAsync(state);
    }

    public async Task UpdateAsync(
        int id,
        StateRequestDTO dto,
        int updatedBy)
    {
        dto.Name = dto.Name.Trim();

        var state = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (state == null)
            throw new KeyNotFoundException("State not found.");

        var duplicate = await _repository
            .Query()
            .AnyAsync(x =>
                x.Id != id &&
                x.Name.ToLower() == dto.Name.ToLower());

        if (duplicate)
            throw new ArgumentException(
                "Another state with this name already exists.");

        state.Name = dto.Name;
        state.UpdatedBy = updatedBy;
        state.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(state);
    }

    public async Task DeleteAsync(
        int id,
        int deletedBy)
    {
        var state = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (state == null)
            throw new KeyNotFoundException("State not found.");

        await _cityrepository
            .SoftDeleteMultipleAsync(
                x => x.StateId == id,
                deletedBy);

        state.DeletedBy = deletedBy;
        state.DeletedAt = DateTime.UtcNow;

        await _repository.SoftDeleteAsync(state);
    }

    public async Task<List<StateResponseDTO>>
        GetAllStatesAsync()
    {
        var states = await _repository
            .Query()
            .Where(x => x.Cities.Any())
            .OrderBy(x => x.Name)
            .ToListAsync();

        return _mapper.Map<List<StateResponseDTO>>(states);
    }


}