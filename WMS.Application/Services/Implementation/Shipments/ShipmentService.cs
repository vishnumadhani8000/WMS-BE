using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Shipments;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Application.Services;

public class ShipmentService : IShipmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommonRepository<Shipment> _shipmentRepository;
    private readonly ICommonRepository<Order> _orderRepository;
    private readonly ICommonRepository<Driver> _driverRepository;
    private readonly ICommonRepository<Vehicle> _vehicleRepository;
    private readonly IMapper _mapper;

    public ShipmentService(
        IUnitOfWork unitOfWork,
        ICommonRepository<Shipment> shipmentRepository,
        ICommonRepository<Order> orderRepository,
        ICommonRepository<Driver> driverRepository,
        ICommonRepository<Vehicle> vehicleRepository,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _shipmentRepository = shipmentRepository;
        _orderRepository = orderRepository;
        _driverRepository = driverRepository;
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task CreateShipmentAsync(
        MakeShipmentRequestDto dto,
        int userId)
    {



        var driver = await _driverRepository.GetByIdAsync(
            dto.DriverId);

        if (driver == null)
        {
            throw new KeyNotFoundException(
                "Driver not found.");
        }

        if (!driver.IsAvailable)
        {
            throw new ArgumentException(
                "Driver is not available.");
        }

        var vehicle = await _vehicleRepository.GetByIdAsync(
            dto.VehicleId);

        if (vehicle == null)
        {
            throw new KeyNotFoundException(
                "Vehicle not found.");
        }

        if (!vehicle.IsAvailable)
        {
            throw new ArgumentException(
                "Vehicle is not available.");
        }
        var orders = await _orderRepository.Query()
            .IgnoreQueryFilters()
            .Include(x => x.Address)
            .Where(x => dto.OrderIds.Contains(x.Id))
            .ToListAsync();

        if (orders.Select(x => x.Address.CityId).Distinct().Count() > 1)
        {
            throw new ArgumentException(
                "All selected orders must beint to the same city.");
        }

        if (orders.Count != dto.OrderIds.Count)
        {
            throw new ArgumentException(
                "One or more orders were not found.");
        }

        if (orders.Any(x => x.ShipmentId != null))
        {
            throw new ArgumentException(
                "One or more orders are already assigned to a shipment.");
        }

        var totalWeightKg = orders.Sum(x => x.TotalWeightKg);

        if (vehicle.CapacityKg < totalWeightKg)
        {
            throw new ArgumentException(
                $"Vehicle capacity is {vehicle.CapacityKg} KG but shipment weight is {totalWeightKg} KG.");
        }


        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var shipment = new Shipment
            {
                DriverId = dto.DriverId,
                VehicleId = dto.VehicleId,
                TotalWeightKg = totalWeightKg,
                CreatedBy = userId
            };

            await _shipmentRepository.AddAsync(shipment);

            foreach (var order in orders)
            {
                order.ShipmentId = shipment.Id;
                order.Status = OrderStatus.Dispatched;
                order.UpdatedBy = userId;
                order.UpdatedAt = DateTime.UtcNow;
            }

            await _orderRepository.UpdateRangeAsync(orders);

            driver.IsAvailable = false;
            driver.UpdatedBy = userId;
            driver.UpdatedAt = DateTime.UtcNow;

            await _driverRepository.UpdateAsync(driver);

            vehicle.IsAvailable = false;
            vehicle.UpdatedBy = userId;
            vehicle.UpdatedAt = DateTime.UtcNow;

            await _vehicleRepository.UpdateAsync(vehicle);

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
    public async Task<PagedResult<ShipmentResponseDto>> GetAllShipmentsAsync(CommonFilterDto filterDto)
    {
        var query = _shipmentRepository.Query()
            .IgnoreQueryFilters()
            .Include(x => x.Driver)
            .Include(x => x.Vehicle)
            .Include(x => x.Orders)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filterDto.Search))
        {
            var search = filterDto.Search.Trim().ToLower();

            query = query.Where(x =>
                x.Driver.Name.ToLower().Contains(search) ||
                x.Vehicle.PlateNumber.ToLower().Contains(search));
        }

        query = filterDto.SortBy?.ToLower() switch
        {
            "shipmentid" => filterDto.Ascending
                ? query.OrderBy(x => x.Id)
                : query.OrderByDescending(x => x.Id),

            "drivername" => filterDto.Ascending
                ? query.OrderBy(x => x.Driver.Name)
                : query.OrderByDescending(x => x.Driver.Name),

            "createdat" => filterDto.Ascending
                ? query.OrderBy(x => x.CreatedAt)
                : query.OrderByDescending(x => x.CreatedAt),

            "vehiclenumber" => filterDto.Ascending
                ? query.OrderBy(x => x.Vehicle.PlateNumber)
                : query.OrderByDescending(x => x.Vehicle.PlateNumber),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var shipments = await query
            .Skip((filterDto.PageNumber - 1) * filterDto.PageSize)
            .Take(filterDto.PageSize)
            .ToListAsync();

        var result = _mapper.Map<List<ShipmentResponseDto>>(shipments);

        return new PagedResult<ShipmentResponseDto>
        {
            Items = result,
            TotalCount = totalCount,
            PageNumber = filterDto.PageNumber,
            PageSize = filterDto.PageSize
        };
    }
    public async Task<ShipmentDetailResponseDto> GetShipmentByIdAsync(int shipmentId)
    {
        var shipment = await _shipmentRepository.Query()
            .Include(x => x.Driver)
            .Include(x => x.Vehicle)
            .Include(x => x.Orders)
                .ThenInclude(x => x.User)
            .Include(x => x.Orders)
                .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.City)
            .Include(x => x.Orders)
                .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.State)
            .FirstOrDefaultAsync(x => x.Id == shipmentId);

        if (shipment == null)
        {
            throw new KeyNotFoundException(
                "Shipment not found.");
        }

        return _mapper.Map<ShipmentDetailResponseDto>(
            shipment);
    }
    public async Task UpdateShipmentStatusAsync(int shipmentId, ShipmentStatus status, int userId)
    {

        var shipment = await _shipmentRepository.Query()
            .IgnoreQueryFilters()
            .Include(x => x.Orders)
            .Include(x => x.Driver)
            .Include(x => x.Vehicle)
            .FirstOrDefaultAsync(x => x.Id == shipmentId);

        if (shipment == null)
        {
            throw new KeyNotFoundException(
                "Shipment not found.");
        }

        if (shipment.Status == ShipmentStatus.Delivered)
        {
            throw new ArgumentException(
                "Delivered shipment cannot be modified.");
        }

        if (shipment.Status == ShipmentStatus.Cancelled)
        {
            throw new ArgumentException(
                "Cancelled shipment cannot be modified.");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {

            shipment.Status = status;
            shipment.UpdatedBy = userId;


            switch (status)
            {
                case ShipmentStatus.InTransit:

                    foreach (var order in shipment.Orders)
                    {
                        order.Status = OrderStatus.InTransit;
                        order.UpdatedBy = userId;
                    }

                    await _orderRepository.UpdateRangeAsync(shipment.Orders);

                    break;

                case ShipmentStatus.Delivered:

                    foreach (var order in shipment.Orders)
                    {
                        order.Status = OrderStatus.Delivered;
                        order.UpdatedBy = userId;

                    }

                    shipment.Driver.IsAvailable = true;
                    shipment.Driver.UpdatedBy = userId;


                    shipment.Vehicle.IsAvailable = true;
                    shipment.Vehicle.UpdatedBy = userId;
                    shipment.Vehicle.UpdatedAt = DateTime.UtcNow;

                    await _orderRepository.UpdateRangeAsync(
                        shipment.Orders);

                    await _driverRepository.UpdateAsync(
                        shipment.Driver);

                    await _vehicleRepository.UpdateAsync(
                        shipment.Vehicle);

                    break;

                case ShipmentStatus.Cancelled:

                    foreach (var order in shipment.Orders)
                    {
                        order.Status = OrderStatus.Accepted;
                        order.ShipmentId = null;
                        order.UpdatedBy = userId;

                    }

                    shipment.Driver.IsAvailable = true;
                    shipment.Driver.UpdatedBy = userId;


                    shipment.Vehicle.IsAvailable = true;
                    shipment.Vehicle.UpdatedBy = userId;


                    await _orderRepository.UpdateRangeAsync(
                        shipment.Orders);

                    await _driverRepository.UpdateAsync(
                        shipment.Driver);

                    await _vehicleRepository.UpdateAsync(
                        shipment.Vehicle);

                    break;
            }

            await _shipmentRepository.UpdateAsync(
                shipment);

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}