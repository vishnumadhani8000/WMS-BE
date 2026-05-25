using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS.Application.DTOs.Products;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Domain.Entities;
using WMS.Shared.Response;

namespace WMS.Application.Services;
public class ProductService : IProductService
{
    private readonly ICommonRepository<Product> _repository;
    private readonly IMapper _mapper;

    public ProductService(
        ICommonRepository<Product> repository,
        IMapper mapper
    )
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResult<BaseProductDto>>> GetAllAsync(CommonFilterDto requestDto)
    {
        IQueryable<Product> query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(requestDto.Search))
        {
            requestDto.Search = requestDto.Search.Trim().ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(requestDto.Search) ||
                (
                    x.Description != null &&
                    x.Description.ToLower().Contains(requestDto.Search)
                )
            );
        }


        query = requestDto.SortBy?.ToLower() switch
        {
            "name" => requestDto.Ascending
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),

            "weight" => requestDto.Ascending
                ? query.OrderBy(x => x.WeightKg)
                : query.OrderByDescending(x => x.WeightKg),

            "stock" => requestDto.Ascending
                ? query.OrderBy(x => x.Stock)
                : query.OrderByDescending(x => x.Stock),
            
            "price" => requestDto.Ascending
                ? query.OrderBy(x => x.Price)
                : query.OrderByDescending(x => x.Price),

            _ => query.OrderByDescending(x => x.CreatedAt),
        };


        var totalCount = await query.CountAsync();

        var products = await query
            .Skip((requestDto.PageNumber - 1) * requestDto.PageSize)
            .Take(requestDto.PageSize)
            .ToListAsync();

        var productDtos =
            _mapper.Map<List<BaseProductDto>>(products);

        var result = new PagedResult<BaseProductDto>
        {
            Items = productDtos,
            TotalCount = totalCount,
            PageNumber = requestDto.PageNumber,
            PageSize = requestDto.PageSize,
        };

        return ApiResponse<PagedResult<BaseProductDto>>
            .Success(
                result,
                "Products fetched successfully."
            );
    }

    public async Task<ApiResponse<BaseProductDto>> GetByIdAsync(long id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return ApiResponse<BaseProductDto>
                .Failure("Product not found.");
        }

        var response =
            _mapper.Map<BaseProductDto>(product);

        return ApiResponse<BaseProductDto>
            .Success(
                response,
                "Product fetched successfully."
            );
    }

    public async Task<ApiResponse<BaseProductDto>> CreateAsync(BaseProductDto dto, long userId)
    {
        var product = _mapper.Map<Product>(dto);

        product.CreatedBy = userId;

        await _repository.AddAsync(product);

        var response =
            _mapper.Map<BaseProductDto>(product);

        return ApiResponse<BaseProductDto>
            .Success(
                response,
                "Product created successfully."
            );
    }

    public async Task<ApiResponse<BaseProductDto>> UpdateAsync(BaseProductDto dto, long userId)
    {

        if (!dto.Id.HasValue)
        {
            return ApiResponse<BaseProductDto>
                .Failure("Product id is required.");
        }

        var product = await _repository.GetByIdAsync(dto.Id.Value);



        if (product == null)
        {
            return ApiResponse<BaseProductDto>
                .Failure("Product not found.");
        }

        // ── Update existing entity ─────────────
        _mapper.Map(dto, product);

        product.UpdatedBy = userId;
        product.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(product);

        var response =
            _mapper.Map<BaseProductDto>(product);

        return ApiResponse<BaseProductDto>
            .Success(
                response,
                "Product updated successfully."
            );
    }

    public async Task<ApiResponse<object>> DeleteAsync(long id, long userId)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            return ApiResponse<object>
                .Failure("Product not found.");
        }

        product.DeletedBy = userId;
        product.DeletedAt = DateTime.UtcNow;

        await _repository.SoftDeleteAsync(product);

        return ApiResponse<object>
            .Success("Product deleted successfully.");
    }

    public async Task<ApiResponse<PagedResult<BaseProductDto>>> GetAllForCustomerAsync(CommonFilterDto requestDto)
    {
        IQueryable<Product> query = _repository.Query();

        query = query.Where(x => x.Stock > 0);

        if (!string.IsNullOrWhiteSpace(requestDto.Search))
        {
            requestDto.Search = requestDto.Search.Trim().ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(requestDto.Search) ||
                (
                    x.Description != null &&
                    x.Description.ToLower().Contains(requestDto.Search)
                )
            );
        }


        query = requestDto.SortBy?.ToLower() switch
        {
            "name" => requestDto.Ascending
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),

            "price" => requestDto.Ascending
                ? query.OrderBy(x => x.Price)
                : query.OrderByDescending(x => x.Price),

            "stock" => requestDto.Ascending
                ? query.OrderBy(x => x.Stock)
                : query.OrderByDescending(x => x.Stock),

            _ => query.OrderByDescending(x => x.CreatedAt),
        };


        var totalCount = await query.CountAsync();

        var products = await query
            .Skip((requestDto.PageNumber - 1) * requestDto.PageSize)
            .Take(requestDto.PageSize)
            .ToListAsync();

        var productDtos =
            _mapper.Map<List<BaseProductDto>>(products);

        var result = new PagedResult<BaseProductDto>
        {
            Items = productDtos,
            TotalCount = totalCount,
            PageNumber = requestDto.PageNumber,
            PageSize = requestDto.PageSize,
        };

        return ApiResponse<PagedResult<BaseProductDto>>
            .Success(
                result,
                "Products fetched successfully."
            );
    }
}