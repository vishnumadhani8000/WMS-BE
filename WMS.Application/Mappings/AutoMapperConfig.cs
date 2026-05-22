    using System.Runtime.InteropServices;
    using AutoMapper;
    using WMS.Application.DTOs.Carts;
    using WMS.Application.DTOs.Products;
    using WMS.Application.DTOs.State;
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
        }

        private void ProductMappings()
        {

            CreateMap<Product, BaseProductDto>();


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
                .ForMember(dest => dest.CartItemId,opt => opt.MapFrom(src => src.Id))
                .ForMember( dest => dest.ProductName,opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.AvailableStock,opt => opt.MapFrom(src => src.Product.Stock))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<Cart, CartResponseDto>()
                .ForMember(
                    dest => dest.CartId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.Items,
                    opt => opt.MapFrom(src => src.CartItems));

        }


    }   