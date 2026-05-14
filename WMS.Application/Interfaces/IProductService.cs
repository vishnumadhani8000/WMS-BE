using WMS.Application.DTOs.Products;
using WMS.Domain.Common;

namespace WMS.Application.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductResponseDto>> GetAllAsync(   int pageNumber,int pageSize, string? search,string? sortBy,bool ascending);

    Task<ProductResponseDto?> GetByIdAsync(long id);

    Task<ProductResponseDto> CreateAsync(CreateProductDto dto , long CreatedByUser);

    Task<bool> UpdateAsync(long id, UpdateProductDto dto , long UpdatedByUser);

    Task<bool> DeleteAsync(long id , long DeletedByUser);
}