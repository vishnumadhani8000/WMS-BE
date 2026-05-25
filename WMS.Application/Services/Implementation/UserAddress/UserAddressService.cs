using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application;
using WMS.Application.Common;
using WMS.Application.DTOs.UserAddresses;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Shared.Response;

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

    public async Task<ApiResponse<IEnumerable<UserAddressResponseDto>>>
        GetUserAddressesAsync(long userId)
    {
        var addresses = await _addressRepository
            .Query()
            .Include(x => x.State)
            .Include(x => x.City)
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var response = _mapper.Map<
            IEnumerable<UserAddressResponseDto>>(addresses);

        return ApiResponse<IEnumerable<UserAddressResponseDto>>
            .Success(response);
    }

    public async Task<ApiResponse<UserAddressResponseDto>>
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
            return ApiResponse<UserAddressResponseDto>
                .Failure("Address not found.");
        }

        var response = _mapper
            .Map<UserAddressResponseDto>(address);

        return ApiResponse<UserAddressResponseDto>
            .Success(response);
    }

    public async Task<ApiResponse<UserAddressResponseDto>> CreateAsync(
        long userId,
        UserAddressRequestDto dto)
    {
        // Validate State
        var stateExists = await _stateRepository
            .Query()
            .AnyAsync(x => x.Id == dto.StateId);

        if (!stateExists)
        {
            return ApiResponse<UserAddressResponseDto>
                .Failure("Invalid state id.");
        }

        // Validate City belongs to State
        var city = await _cityRepository
            .Query()
            .FirstOrDefaultAsync(x =>
                x.Id == dto.CityId &&
                x.StateId == dto.StateId);

        if (city == null)
        {
            return ApiResponse<UserAddressResponseDto>
                .Failure("Invalid city id for selected state.");
        }

        var address = _mapper.Map<UserAddress>(dto);

        address.UserId = userId;

        await _addressRepository.AddAsync(address);

        var createdAddress = await _addressRepository
            .Query()
            .Include(x => x.State)
            .Include(x => x.City)
            .FirstAsync(x => x.Id == address.Id);

        var response = _mapper.Map<UserAddressResponseDto>(createdAddress);

        return ApiResponse<UserAddressResponseDto>
            .Success(response);
    }

    public async Task<ApiResponse<string>> UpdateAsync(
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
            return ApiResponse<string>
                .Failure("Address not found.");
        }

        _mapper.Map(dto, address);

        await _addressRepository.UpdateAsync(address);

        return ApiResponse<string>
            .Success("Address updated successfully.");
    }

    public async Task<ApiResponse<string>> DeleteAsync(
        long addressId,
        long userId)
    {
        var address = await _addressRepository
            .GetFirstOrDefaultAsync(x =>
                x.Id == addressId &&
                x.UserId == userId);

        if (address == null)
        {
            return ApiResponse<string>
                .Failure("Address not found.");
        }

        await _addressRepository.SoftDeleteAsync(address);

        return ApiResponse<string>
            .Success("Address deleted successfully.");
    }
}