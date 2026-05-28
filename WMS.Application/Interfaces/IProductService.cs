using WMS.Application.DTOs.Products;
using WMS.Domain.Common;

public interface IProductService
{
    Task<PagedResult<BaseProductDto>> GetAllAsync(CommonFilterDto requestDto);

    Task<BaseProductDto> GetByIdAsync(long id);

    Task<PagedResult<ProductResponseCustomerDto>>GetAllForCustomerAsync(CommonFilterDto requestDto);

    Task<BaseProductDto> CreateAsync(BaseProductDto dto,long createdByUser);

    Task<BaseProductDto> UpdateAsync(BaseProductDto dto,long updatedByUser);

    Task DeleteAsync(long id,long deletedByUser);
}