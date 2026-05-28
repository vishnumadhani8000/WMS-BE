
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Vehicles;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly ICommonRepository<Vehicle> _repository;
    private readonly IMapper _mapper;

    public VehicleService(
        ICommonRepository<Vehicle> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<VehicleResponseDto>> GetAllAsync(
        CommonFilterDto requestDto)
    {
        IQueryable<Vehicle> query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(requestDto.Search))
        {
            requestDto.Search = requestDto.Search
                .Trim()
                .ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(requestDto.Search) ||
                x.PlateNumber.ToLower().Contains(requestDto.Search));
        }

        query = requestDto.SortBy?.ToLower() switch
        {
            "name" => requestDto.Ascending
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),

            "capacitykg" => requestDto.Ascending
                ? query.OrderBy(x => x.CapacityKg)
                : query.OrderByDescending(x => x.CapacityKg),

            "platenumber" => requestDto.Ascending
                ? query.OrderBy(x => x.PlateNumber)
                : query.OrderByDescending(x => x.PlateNumber),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var vehicles = await query
            .Skip((requestDto.PageNumber - 1) * requestDto.PageSize)
            .Take(requestDto.PageSize)
            .ToListAsync();

        return new PagedResult<VehicleResponseDto>
        {
            Items = _mapper.Map<List<VehicleResponseDto>>(vehicles),
            TotalCount = totalCount,
            PageNumber = requestDto.PageNumber,
            PageSize = requestDto.PageSize
        };
    }

    public async Task<VehicleResponseDto> GetByIdAsync(long id)
    {
        var vehicle = await _repository.GetByIdAsync(id);

        if (vehicle == null)
        {
            throw new KeyNotFoundException(
                "Vehicle not found.");
        }

        return _mapper.Map<VehicleResponseDto>(vehicle);
    }

    public async Task<VehicleResponseDto> CreateAsync(
        VehicleRequestDto dto,
        long userId)
    {
        dto.PlateNumber = dto.PlateNumber
            .Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .ToUpper();

        var exists = await _repository
            .Query()
            .AnyAsync(x =>
                x.PlateNumber.ToLower() ==
                dto.PlateNumber.ToLower());

        if (exists)
        {
            throw new ArgumentException(
                "Vehicle plate number already exists.");
        }

        var vehicle = _mapper.Map<Vehicle>(dto);

        vehicle.CreatedBy = userId;

        await _repository.AddAsync(vehicle);

        return _mapper.Map<VehicleResponseDto>(vehicle);
    }

    public async Task<VehicleResponseDto> UpdateAsync(
        long id,
        VehicleRequestDto dto,
        long userId)
    {
        dto.PlateNumber = dto.PlateNumber
            .Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .ToUpper();

        var vehicle = await _repository.GetByIdAsync(id);

        if (vehicle == null)
        {
            throw new KeyNotFoundException(
                "Vehicle not found.");
        }

        var exists = await _repository
            .Query()
            .AnyAsync(x =>
                x.Id != id &&
                x.PlateNumber.ToLower() ==
                dto.PlateNumber.ToLower());

        if (exists)
        {
            throw new ArgumentException(
                "Vehicle plate number already exists.");
        }

        _mapper.Map(dto, vehicle);

        vehicle.UpdatedBy = userId;
        vehicle.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(vehicle);

        return _mapper.Map<VehicleResponseDto>(vehicle);
    }

    public async Task DeleteAsync(
        long id,
        long userId)
    {
        var vehicle = await _repository.GetByIdAsync(id);

        if (vehicle == null)
        {
            throw new KeyNotFoundException(
                "Vehicle not found.");
        }

        vehicle.DeletedBy = userId;
        vehicle.DeletedAt = DateTime.UtcNow;

        await _repository.SoftDeleteAsync(vehicle);
    }
}

