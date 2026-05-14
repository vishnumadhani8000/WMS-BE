using AutoMapper;

using WMS.Application.DTOs.Products;
using WMS.Domain.Entities;

namespace WMS.Application.Common.Mappings;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        ProductMappings();
    }

    private void ProductMappings()
    {
       
        CreateMap<Product, ProductResponseDto>();


        CreateMap<CreateProductDto, Product>()
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


        CreateMap<UpdateProductDto, Product>()
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


}