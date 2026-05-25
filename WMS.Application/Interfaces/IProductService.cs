using Microsoft.AspNetCore.Mvc.ApiExplorer;
using WMS.Application.DTOs.Products;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IProductService
{
    Task<ApiResponse<PagedResult<BaseProductDto>>> GetAllAsync(CommonFilterDto requestDto);

    Task<ApiResponse<BaseProductDto>> GetByIdAsync(long id );
    Task<ApiResponse<PagedResult<ProductResponseCustomerDto>>> GetAllForCustomerAsync(CommonFilterDto requestDto);


    Task<ApiResponse<BaseProductDto>> CreateAsync( BaseProductDto dto,long createdByUser);

    Task<ApiResponse<BaseProductDto>> UpdateAsync(BaseProductDto dto,long updatedByUser);

    Task<ApiResponse<object>> DeleteAsync(long id,long deletedByUser);
}