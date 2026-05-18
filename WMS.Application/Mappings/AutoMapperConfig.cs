using AutoMapper;

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
    }

    private void ProductMappings()
    {

        CreateMap<Product, ProductResponseDto>();


        CreateMap<ProductRequestDto, Product>()
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
        CreateMap<State, StateResponceListDTO>();

        CreateMap<StateRequestDTO, State>()
            .ForMember(dest => dest.Id,        opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt,  opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy,  opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt,  opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy,  opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt,  opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy,  opt => opt.Ignore())
            .ForMember(dest => dest.Cities,     opt => opt.Ignore())
            .ForMember(dest => dest.Addresses,  opt => opt.Ignore());
    }

}