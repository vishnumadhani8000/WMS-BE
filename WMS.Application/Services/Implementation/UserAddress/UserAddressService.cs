
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application;
using WMS.Application.Common;
using WMS.Application.DTOs.UserAddresses;
using WMS.Application.Services;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Services;

public class UserAddressService : IUserAddressService
{
    private readonly ICommonRepository<UserAddress> _addressRepository;
    private readonly ICommonRepository<State> _stateRepository;
    private readonly ICommonRepository<City> _cityRepository;
    private readonly IMapper _mapper;

    public UserAddressService(
        ICommonRepository<UserAddress> addressRepository,
        ICommonRepository<State> stateRepository,
        ICommonRepository<City> cityRepository,
        IMapper mapper)
    {
        _addressRepository = addressRepository;
        _stateRepository = stateRepository;
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserAddressResponseDto>>
        GetUserAddressesAsync(long userId)
    {
        var addresses = await _addressRepository
            .Query()
            .Include(x => x.State)
            .Include(x => x.City)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return _mapper.Map<
            IEnumerable<UserAddressResponseDto>>(addresses);
    }

    public async Task<UserAddressResponseDto>
        GetByIdAsync(long addressId, long userId)
    {
        var address = await _addressRepository
            .Query()
            .Include(x => x.State)
            .Include(x => x.City)
            .FirstOrDefaultAsync(x =>
                x.Id == addressId &&
                x.UserId == userId);

        if (address == null)
        {
            throw new KeyNotFoundException(
                "Address not found.");
        }

        return _mapper.Map<UserAddressResponseDto>(address);
    }

    public async Task<UserAddressResponseDto> CreateAsync(
        long userId,
        UserAddressRequestDto dto)
    {
        var stateExists = await _stateRepository
            .ExistsAsync(x => x.Id == dto.StateId);

        if (!stateExists)
        {
            throw new ArgumentException(
                "Invalid state id.");
        }

        var city = await _cityRepository
            .Query()
            .FirstOrDefaultAsync(x =>
                x.Id == dto.CityId &&
                x.StateId == dto.StateId);

        if (city == null)
        {
            throw new ArgumentException(
                "Invalid city id for selected state.");
        }

        var address = _mapper.Map<UserAddress>(dto);
        address.UserId = userId;

        await _addressRepository.AddAsync(address);

        var createdAddress = await _addressRepository
            .Query()
            .Include(x => x.State)
            .Include(x => x.City)
            .FirstAsync(x => x.Id == address.Id);

        return _mapper.Map<UserAddressResponseDto>(
            createdAddress);
    }

    public async Task UpdateAsync(
        long addressId,
        long userId,
        UserAddressRequestDto dto)
    {
        var address = await _addressRepository
            .GetFirstOrDefaultAsync(x =>
                x.Id == addressId &&
                x.UserId == userId);

        if (address == null)
        {
            throw new KeyNotFoundException(
                "Address not found.");
        }

        _mapper.Map(dto, address);

        await _addressRepository.UpdateAsync(address);
    }

    public async Task DeleteAsync(
        long addressId,
        long userId)
    {
        var address = await _addressRepository
            .GetFirstOrDefaultAsync(x =>
                x.Id == addressId &&
                x.UserId == userId);

        if (address == null)
        {
            throw new KeyNotFoundException(
                "Address not found.");
        }

        await _addressRepository.SoftDeleteAsync(address);
    }
}

