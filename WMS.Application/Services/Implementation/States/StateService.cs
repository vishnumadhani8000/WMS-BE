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
    private readonly IMapper _mapper;

    public StateService(
        ICommonRepository<State> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResult<StateResponceListDTO>>> GetAllAsync(
        int pageNumber,
        int pageSize)
    {
        IQueryable<State> query = _repository
            .Query();

        var totalCount = await query.CountAsync();

        var states = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PagedResult<StateResponceListDTO>
        {
            Items = _mapper.Map<List<StateResponceListDTO>>(states),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        return ApiResponse<PagedResult<StateResponceListDTO>>
            .Success(result, "States fetched successfully.");
    }

    public async Task<ApiResponse<StateResponseDTO>> GetByIdAsync(long id)
    {
        var state = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (state == null)
            return ApiResponse<StateResponseDTO>
                .Failure("State not found.");

        var response = _mapper.Map<StateResponseDTO>(state);

        return ApiResponse<StateResponseDTO>
            .Success(response, "State fetched successfully.");
    }

    public async Task<ApiResponse<bool>> CreateAsync(
        StateRequestDTO dto,
        long createdBy)
    {
        dto.Name = dto.Name.Trim();

        var exists = await _repository.ExistsAsync(
             x => x.Name.ToLower() == dto.Name.ToLower());

        if (exists)
            return ApiResponse<bool>
                .Failure("State with this name already exists.");

        var state = _mapper.Map<State>(dto);
        state.CreatedBy = createdBy;

        await _repository.AddAsync(state);

        return ApiResponse<bool>
            .Success(true, "State created successfully.");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(
        long id,
        StateRequestDTO dto,
        long updatedBy)
    {
        dto.Name = dto.Name.Trim();

        var state = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (state == null)
            return ApiResponse<bool>
                .Failure("State not found.");

        var duplicate = await _repository
            .Query()
            .AnyAsync(x =>
                x.Id != id &&
                x.Name.ToLower() == dto.Name.ToLower()
            );

        if (duplicate)
            return ApiResponse<bool>
                .Failure("Another state with this name already exists.");

        state.Name = dto.Name;
        state.UpdatedBy = updatedBy;
        state.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(state);

        return ApiResponse<bool>
            .Success(true, "State updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(
        long id,
        long deletedBy)
    {
        var state = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (state == null)
            return ApiResponse<bool>
                .Failure("State not found.");

        state.DeletedBy = deletedBy;
        state.DeletedAt = DateTime.UtcNow;

        await _repository.SoftDeleteAsync(state);

        return ApiResponse<bool>
            .Success(true, "State deleted successfully.");
    }
}