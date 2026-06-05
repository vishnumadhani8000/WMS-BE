using WMS.Application.DTOs.Products;
using WMS.Domain.Common;

public interface IProductService
{
    Task<PagedResult<BaseProductDto>> GetAllAsync(CommonFilterDto requestDto);

    Task<BaseProductDto> GetByIdAsync(int id);

    Task<PagedResult<ProductResponseCustomerDto>>GetAllForCustomerAsync(CommonFilterDto requestDto);

    Task<BaseProductDto> CreateAsync(BaseProductDto dto,int createdByUser);

    Task<BaseProductDto> UpdateAsync(BaseProductDto dto,int updatedByUser);

    Task DeleteAsync(int id,int deletedByUser);
}