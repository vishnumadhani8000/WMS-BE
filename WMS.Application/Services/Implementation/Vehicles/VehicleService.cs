using AutoMapper;
using Microsoft.EntityFrameworkCore;

using WMS.Application.DTOs.Vehicles;
using WMS.Application.Interfaces;

using WMS.Domain.Common;
using WMS.Domain.Entities;

using WMS.Shared.Response;

namespace WMS.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly ICommonRepository<Vehicle> _repository;
    private readonly IMapper _mapper;

    public VehicleService(
        ICommonRepository<Vehicle> repository,
        IMapper mapper
    )
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResult<VehicleResponseDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? sortBy,
        bool ascending
    )
    {
        IQueryable<Vehicle> query = _repository.Query();

        // Search
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(search) ||
                x.PlateNumber.ToLower().Contains(search)
            );
        }

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "name" => ascending
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),

            "capacity" => ascending
                ? query.OrderBy(x => x.CapacityKg)
                : query.OrderByDescending(x => x.CapacityKg),

            "plate" => ascending
                ? query.OrderBy(x => x.PlateNumber)
                : query.OrderByDescending(x => x.PlateNumber),

            _ => query.OrderByDescending(x => x.CreatedAt),
        };

        var totalCount = await query.CountAsync();

        var vehicles = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var vehicleDtos =
            _mapper.Map<List<VehicleResponseDto>>(vehicles);

        var result = new PagedResult<VehicleResponseDto>
        {
            Items = vehicleDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        return ApiResponse<PagedResult<VehicleResponseDto>>
            .Success(result, "Vehicles fetched successfully.");
    }

    public async Task<ApiResponse<VehicleResponseDto>> GetByIdAsync(
        long id
    )
    {
        var vehicle = await _repository.GetByIdAsync(id);

        if (vehicle == null)
        {
            return ApiResponse<VehicleResponseDto>
                .Failure("Vehicle not found.");
        }

        var response =
            _mapper.Map<VehicleResponseDto>(vehicle);

        return ApiResponse<VehicleResponseDto>
            .Success(response, "Vehicle fetched successfully.");
    }

    public async Task<ApiResponse<VehicleResponseDto>> CreateAsync(
    VehicleRequestDto dto,
    long userId
)
    {
        // Normalize plate number
        dto.PlateNumber = dto.PlateNumber
            ?.Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .ToUpper();

        var exists = await _repository
            .Query()
            .AnyAsync(x =>
                x.PlateNumber.ToLower() ==
                dto.PlateNumber.ToLower()
            );

        if (exists)
        {
            return ApiResponse<VehicleResponseDto>
                .Failure(
                    "Vehicle plate number already exists."
                );
        }

        var vehicle = _mapper.Map<Vehicle>(dto);

        vehicle.CreatedBy = userId;

        await _repository.AddAsync(vehicle);

        var response =
            _mapper.Map<VehicleResponseDto>(vehicle);

        return ApiResponse<VehicleResponseDto>
            .Success(
                response,
                "Vehicle created successfully."
            );
    }

    public async Task<ApiResponse<object>> UpdateAsync(
    long id,
    VehicleRequestDto dto,
    long userId
)
    {
        // Normalize plate number
        dto.PlateNumber = dto.PlateNumber
            ?.Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .ToUpper();

        var vehicle = await _repository.GetByIdAsync(id);

        if (vehicle == null)
        {
            return ApiResponse<object>
                .Failure("Vehicle not found.");
        }

        var exists = await _repository
            .Query()
            .AnyAsync(x =>
                x.Id != id &&
                x.PlateNumber.ToLower() ==
                dto.PlateNumber.ToLower()
            );

        if (exists)
        {
            return ApiResponse<object>
                .Failure(
                    "Vehicle plate number already exists."
                );
        }

        _mapper.Map(dto, vehicle);

        vehicle.UpdatedBy = userId;
        vehicle.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(vehicle);

        return ApiResponse<object>
            .Success("Vehicle updated successfully.");
    }
    public async Task<ApiResponse<object>> DeleteAsync(
            long id,
            long userId
        )
    {
        var vehicle = await _repository.GetByIdAsync(id);

        if (vehicle == null)
        {
            return ApiResponse<object>
                .Failure("Vehicle not found.");
        }

        vehicle.DeletedBy = userId;
        vehicle.DeletedAt = DateTime.UtcNow;

        await _repository.SoftDeleteAsync(vehicle);

        return ApiResponse<object>
            .Success("Vehicle deleted successfully.");
    }
}