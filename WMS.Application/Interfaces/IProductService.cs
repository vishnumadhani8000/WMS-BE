using WMS.Application.DTOs.Products;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IProductService
{
    Task<ApiResponse<PagedResult<ProductResponseDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? sortBy,
        bool ascending
    );

    Task<ApiResponse<ProductResponseDto>> GetByIdAsync(
        long id
    );

    Task<ApiResponse<ProductResponseDto>> CreateAsync(
        ProductRequestDto dto,
        long createdByUser
    );

    Task<ApiResponse<ProductResponseDto>> UpdateAsync(
        long id,
        ProductRequestDto dto,
        long updatedByUser
    );

    Task<ApiResponse<object>> DeleteAsync(
        long id,
        long deletedByUser
    );
}