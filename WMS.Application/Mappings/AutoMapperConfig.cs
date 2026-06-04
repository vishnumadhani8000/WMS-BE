using AutoMapper;
using WMS.Application.DTOs.Carts;
using WMS.Application.DTOs.Drivers;
using WMS.Application.DTOs.Order;
using WMS.Application.DTOs.Products;
using WMS.Application.DTOs.Shipments;
using WMS.Application.DTOs.State;
using WMS.Application.DTOs.UserAddresses;
using WMS.Application.DTOs.Vehicles;
using WMS.Domain.Entities;

namespace WMS.Application.Common.Mappings;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        ProductMappings();
        VehicleMappings();
        StateMappings();
        CityMappings();
        CartMappings();
        UserAddressMappings();
        OrderMappings();
        DriverMappings();
        ShipmentMappings();
    }

    private void ProductMappings()
    {

        CreateMap<Product, BaseProductDto>();
        CreateMap<Product, ProductResponseCustomerDto>();


        CreateMap<BaseProductDto, Product>()
        .ForMember(
            dest => dest.Id,
            opt => opt.Ignore()
        )
        .ForMember(
            dest => dest.CreatedAt,
            opt => opt.Ignore()
        )
        .ForMember(
            dest => dest.CreatedBy,
            opt => opt.Ignore()
        )
        .ForMember(
            dest => dest.UpdatedAt,
            opt => opt.Ignore()
        )
        .ForMember(
            dest => dest.UpdatedBy,
            opt => opt.Ignore()
        )
        .ForMember(
            dest => dest.IsDeleted,
            opt => opt.Ignore()
        )
        .ForMember(
            dest => dest.DeletedAt,
            opt => opt.Ignore()
        )
        .ForMember(
            dest => dest.DeletedBy,
            opt => opt.Ignore()
        );
    }
    private void VehicleMappings()
    {
        CreateMap<Vehicle, VehicleResponseDto>();
        CreateMap<Vehicle, AvailableVehicleDto>();

        CreateMap<VehicleRequestDto, Vehicle>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());
    }

    private void StateMappings()
    {
        CreateMap<State, StateResponseDTO>();
        CreateMap<StateRequestDTO, State>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Cities, opt => opt.Ignore())
            .ForMember(dest => dest.Addresses, opt => opt.Ignore());
    }
    private void CityMappings()
    {
        CreateMap<City, CityResponseDTO>();
        CreateMap<CityRequestDTO, City>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Addresses, opt => opt.Ignore());
    }

    private void CartMappings()
    {

        CreateMap<CartItem, CartItemResponseDto>()
            .ForMember(dest => dest.CartItemId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.AvailableStock, opt => opt.MapFrom(src => src.Product.Stock))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

        CreateMap<Cart, CartResponseDto>()
            .ForMember(
                dest => dest.CartId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src.CartItems));

    }

    private void UserAddressMappings()
    {
        CreateMap<UserAddress, UserAddressResponseDto>()
            .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.State.Name))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name));

        CreateMap<UserAddressRequestDto, UserAddress>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())

            .ForMember(dest => dest.IsDefault, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.State, opt => opt.Ignore())
            .ForMember(dest => dest.City, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore());
    }

    private void OrderMappings()
    {
        CreateMap<OrderItem, OrderItemResponseDto>()
            .ForMember(dest => dest.OrderItemId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name));

        CreateMap<Order, OrderResponseDto>()
            .ForMember(dest => dest.OrderId,
                opt => opt.MapFrom(src => src.Id))

            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address))

            .ForMember(dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt))

            .ForMember(dest => dest.Items,
                opt => opt.MapFrom(src => src.OrderItems));
        CreateMap<Order, AdminOrderResponseDto>()
        .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User.Name))
        .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Address.City.Name))
        .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.Address.State.Name))
        .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.OrderItems.Sum(x => x.Quantity)));

        CreateMap<Order, AdminOrderDetailResponseDto>()
        .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))

        .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User.Name))
        .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.Phone))
        .ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src => src.Address.AddressLine))
        .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Address.Pincode))
        .ForMember(dest => dest.Landmark, opt => opt.MapFrom(src => src.Address.Landmark))
        .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Address.City.Name))
        .ForMember(dest => dest.StateName, opt => opt.MapFrom(src => src.Address.State.Name))
        .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));



    }
    private void DriverMappings()
    {
        CreateMap<Driver, DriverResponseDto>();

        CreateMap<Driver, AvailableDriversDto>();

        CreateMap<DriverRequestDto, Driver>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.Shipments, opt => opt.Ignore());


    }
    private void ShipmentMappings()
    {
        CreateMap<Shipment, ShipmentResponseDto>()
            .ForMember(
                dest => dest.ShipmentId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.DriverName,
                opt => opt.MapFrom(src => src.Driver.Name))
            .ForMember(
                dest => dest.VehicleNumber,
                opt => opt.MapFrom(src => src.Vehicle.PlateNumber));

        CreateMap<Order, ShipmentOrderDto>()
            .ForMember(
                dest => dest.OrderId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.User.Name))
            .ForMember(
                dest => dest.CityName,
                opt => opt.MapFrom(src => src.Address.City.Name))
            .ForMember(
                dest => dest.StateName,
                opt => opt.MapFrom(src => src.Address.State.Name));
    

        CreateMap<Shipment, ShipmentDetailResponseDto>()
            .ForMember(
                dest => dest.ShipmentId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.DriverName,
                opt => opt.MapFrom(src => src.Driver.Name))
            .ForMember(
                dest => dest.DriverPhone,
                opt => opt.MapFrom(src => src.Driver.Phone))
            .ForMember(
                dest => dest.VehicleNumber,
                opt => opt.MapFrom(src => src.Vehicle.PlateNumber))
            .ForMember(
                dest => dest.Orders,
                opt => opt.MapFrom(src => src.Orders));
    }


}