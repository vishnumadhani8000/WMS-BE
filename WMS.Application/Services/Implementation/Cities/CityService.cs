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

    public async Task<ApiResponse<PagedResult<CityResponseDTO>>> GetAllAsync(CityFilterRequestDTO requestDTO)
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
        if(!stateExists)
        {
            return ApiResponse<PagedResult<CityResponseDTO>>
                .Failure("State not found.");
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

        return ApiResponse<PagedResult<CityResponseDTO>>
            .Success(result, "Citiess fetched successfully.");
    }

    public async Task<ApiResponse<CityResponseDTO>> GetByIdAsync(long id)
    {
        var city = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (city == null)
            return ApiResponse<CityResponseDTO>
                .Failure("City not found.");

        var response = _mapper.Map<CityResponseDTO>(city);

        return ApiResponse<CityResponseDTO>
            .Success(response, "City fetched successfully.");
    }

    public async Task<ApiResponse<bool>> CreateAsync(
        CityRequestDTO dto,
        long createdBy)
    {
        dto.Name = dto.Name.Trim();


        var stateExists = await _stateRepository
            .ExistsAsync(x => x.Id == dto.StateId);


        if(!stateExists)
            return ApiResponse<bool>
                .Failure("State not found.");

        var exists = await _repository.ExistsAsync(
             x => x.Name.ToLower() == dto.Name.ToLower());

        if (exists)
            return ApiResponse<bool>
                .Failure("City with this name already exists.");

        var city = _mapper.Map<City>(dto);
        city.CreatedBy = createdBy;

        await _repository.AddAsync(city);

        return ApiResponse<bool>
            .Success(true, "City created successfully.");
    }

    public async Task<ApiResponse<bool>> UpdateAsync(
        long id,
        CityRequestDTO dto,
        long updatedBy)
    {
        dto.Name = dto.Name.Trim();

        var city = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (city == null)
            return ApiResponse<bool>
                .Failure("City not found.");

        var duplicate = await _repository
            .Query()
            .AnyAsync(x =>
                x.Id != id &&
                x.Name.ToLower() == dto.Name.ToLower()
            );

        if (duplicate)
            return ApiResponse<bool>
                .Failure("Another city with this name already exists.");

        city.Name = dto.Name;
        city.UpdatedBy = updatedBy;
        city.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(city);

        return ApiResponse<bool>
            .Success(true, "City updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(
        long id,
        long deletedBy)
    {
        var city = await _repository
            .Query()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (city == null)
            return ApiResponse<bool>
                .Failure("City not found.");

        city.DeletedBy = deletedBy;
        city.DeletedAt = DateTime.UtcNow;

        await _repository.SoftDeleteAsync(city);

        return ApiResponse<bool>
            .Success(true, "City deleted successfully.");
    }
    public async Task<ApiResponse<List<CityResponseDTO>>> 
    GetCitiesByStateAsync(long stateId)
{
    var stateExists = await _stateRepository
        .ExistsAsync(x => x.Id == stateId);

    if (!stateExists)
    {
        return ApiResponse<List<CityResponseDTO>>
            .Failure("State not found.");
    }

    var cities = await _repository
        .Query()
        .Where(x => x.StateId == stateId)
        .OrderBy(x => x.Name)
        .ToListAsync();

    var response = _mapper.Map<List<CityResponseDTO>>(cities);

    return ApiResponse<List<CityResponseDTO>>
        .Success(response, "Cities fetched successfully.");
}

 
}