using Microsoft.AspNetCore.Mvc.ApiExplorer;
using WMS.Application.DTOs.Products;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IProductService
{
    Task<ApiResponse<PagedResult<ProductResponseDto>>> GetAllAsync(CommonFilterDto requestDto);

    Task<ApiResponse<ProductResponseDto>> GetByIdAsync(long id );
    Task<ApiResponse<PagedResult<ProductResponseDto>>> GetAllForCustomerAsync(CommonFilterDto requestDto);


    Task<ApiResponse<ProductResponseDto>> CreateAsync( ProductRequestDto dto,long createdByUser);

    Task<ApiResponse<ProductResponseDto>> UpdateAsync(long id ,ProductRequestDto dto,long updatedByUser);

    Task<ApiResponse<object>> DeleteAsync(long id,long deletedByUser);
}