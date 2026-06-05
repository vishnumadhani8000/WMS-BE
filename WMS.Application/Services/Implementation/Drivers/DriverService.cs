using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Drivers;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class DriverService : IDriverService
{
    private readonly ICommonRepository<Driver> _repository;
    private readonly IMapper _mapper;

    public DriverService(
        ICommonRepository<Driver> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<DriverResponseDto>> GetAllAsync(
        CommonFilterDto requestDto)
    {
        IQueryable<Driver> query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(requestDto.Search))
        {
            requestDto.Search = requestDto.Search
                .Trim()
                .ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(requestDto.Search) ||
                x.Phone.ToLower().Contains(requestDto.Search) ||
                (x.LicenceNo != null &&
                 x.LicenceNo.ToLower().Contains(requestDto.Search)));
        }

        query = requestDto.SortBy?.ToLower() switch
        {
            "name" => requestDto.Ascending
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),

            "phone" => requestDto.Ascending
                ? query.OrderBy(x => x.Phone)
                : query.OrderByDescending(x => x.Phone),

            "licenceno" => requestDto.Ascending
                ? query.OrderBy(x => x.LicenceNo)
                : query.OrderByDescending(x => x.LicenceNo),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var drivers = await query
            .Skip((requestDto.PageNumber - 1) * requestDto.PageSize)
            .Take(requestDto.PageSize)
            .ToListAsync();

        return new PagedResult<DriverResponseDto>
        {
            Items = _mapper.Map<List<DriverResponseDto>>(drivers),
            TotalCount = totalCount,
            PageNumber = requestDto.PageNumber,
            PageSize = requestDto.PageSize
        };
    }

    public async Task<DriverResponseDto> GetByIdAsync(int id)
    {
        var driver = await _repository.GetByIdAsync(id);

        if (driver == null)
        {
            throw new KeyNotFoundException(
                "Driver not found.");
        }

        return _mapper.Map<DriverResponseDto>(driver);
    }

    public async Task<DriverResponseDto> CreateAsync(
    DriverRequestDto dto,
    int userId)
    {
        dto.Phone = dto.Phone.Trim();
        dto.LicenceNo = dto.LicenceNo?
           .Trim()
           .Replace(" ", "")
           .ToUpper();

        var phoneExists = await _repository.ExistsAsync(
            x => x.Phone == dto.Phone);

        if (phoneExists)
        {
            throw new ArgumentException(
                "Driver phone number already exists.");
        }

        if (!string.IsNullOrWhiteSpace(dto.LicenceNo))
        {
            var licenceExists = await _repository.ExistsAsync(
                x => x.LicenceNo == dto.LicenceNo);

            if (licenceExists)
            {
                throw new ArgumentException(
                    "Driver licence number already exists.");
            }
        }

        var driver = _mapper.Map<Driver>(dto);

        driver.CreatedBy = userId;

        await _repository.AddAsync(driver);

        return _mapper.Map<DriverResponseDto>(driver);
    }
    public async Task<DriverResponseDto> UpdateAsync(
        int id,
        DriverRequestDto dto,
        int userId)
    {
        dto.Phone = dto.Phone.Trim();
        dto.LicenceNo = dto.LicenceNo?
           .Trim()
           .Replace(" ", "")
           .ToUpper();

        var driver = await _repository.GetByIdAsync(id);

        if (driver == null)
        {
            throw new KeyNotFoundException(
                "Driver not found.");
        }

        var phoneExists = await _repository.ExistsAsync(
            x => x.Id != id &&
                 x.Phone == dto.Phone);

        if (phoneExists)
        {
            throw new ArgumentException(
                "Driver phone number already exists.");
        }

        if (!string.IsNullOrWhiteSpace(dto.LicenceNo))
        {
            var licenceExists = await _repository.ExistsAsync(
                x => x.Id != id &&
                     x.LicenceNo == dto.LicenceNo);

            if (licenceExists)
            {
                throw new ArgumentException(
                    "Driver licence number already exists.");
            }
        }

        _mapper.Map(dto, driver);

        driver.UpdatedBy = userId;
        driver.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(driver);

        return _mapper.Map<DriverResponseDto>(driver);
    }
    public async Task DeleteAsync(
        int id,
        int userId)
    {
        var driver = await _repository.GetByIdAsync(id);

        if (driver == null)
        {
            throw new KeyNotFoundException(
                "Driver not found.");
        }

        if(driver.IsAvailable == false)
        {
            throw new ArgumentException(
                "Cannot delete the driver because they are currently assigned to a shipment.");
        }

        driver.DeletedBy = userId;
        driver.DeletedAt = DateTime.UtcNow;

        await _repository.SoftDeleteAsync(driver);
    }
    public async Task<List<AvailableDriversDto>>GetAvailableDriversAsync()
    {
        var drivers = await _repository.Query()
            .Where(x => x.IsAvailable)
            .OrderBy(x => x.Name)
            .ToListAsync();

        if (!drivers.Any())
        {
            throw new KeyNotFoundException(
                "No drivers available.");
        }

        return _mapper.Map<List<AvailableDriversDto>>(drivers);
    }

  
}