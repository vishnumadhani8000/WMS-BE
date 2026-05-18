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

    public async Task<ApiResponse<PagedResult<ProductResponseDto>>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? sortBy,
        bool ascending
    )
    {
        IQueryable<Product> query = _repository.Query();

        // ── Search ─────────────────────────────
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(search) ||
                (
                    x.Description != null &&
                    x.Description.ToLower().Contains(search)
                )
            );
        }

        // ── Sorting ────────────────────────────
        query = sortBy?.ToLower() switch
        {
            "name" => ascending
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),

            "weight" => ascending
                ? query.OrderBy(x => x.WeightKg)
                : query.OrderByDescending(x => x.WeightKg),

            "stock" => ascending
                ? query.OrderBy(x => x.Stock)
                : query.OrderByDescending(x => x.Stock),

            _ => query.OrderByDescending(x => x.CreatedAt),
        };

        // ── Total Count ────────────────────────
        var totalCount = await query.CountAsync();

        // ── Pagination ─────────────────────────
        var products = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // ── Mapping ────────────────────────────
        var productDtos =
            _mapper.Map<List<ProductResponseDto>>(products);

        var result = new PagedResult<ProductResponseDto>
        {
            Items = productDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        return ApiResponse<PagedResult<ProductResponseDto>>
            .Success(
                result,
                "Products fetched successfully."
            );
    }

    public async Task<ApiResponse<ProductResponseDto>> GetByIdAsync(
        long id
    )
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            return ApiResponse<ProductResponseDto>
                .Failure("Product not found.");
        }

        var response =
            _mapper.Map<ProductResponseDto>(product);

        return ApiResponse<ProductResponseDto>
            .Success(
                response,
                "Product fetched successfully."
            );
    }

    public async Task<ApiResponse<ProductResponseDto>> CreateAsync(
        ProductRequestDto dto,
        long userId
    )
    {
        var product = _mapper.Map<Product>(dto);

        product.CreatedBy = userId;

        await _repository.AddAsync(product);

        var response =
            _mapper.Map<ProductResponseDto>(product);

        return ApiResponse<ProductResponseDto>
            .Success(
                response,
                "Product created successfully."
            );
    }

    public async Task<ApiResponse<ProductResponseDto>> UpdateAsync(
        long id,
        ProductRequestDto dto,
        long userId
    )
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            return ApiResponse<ProductResponseDto>
                .Failure("Product not found.");
        }

        // ── Update existing entity ─────────────
        _mapper.Map(dto, product);

        product.UpdatedBy = userId;
        product.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(product);

        var response =
            _mapper.Map<ProductResponseDto>(product);

        return ApiResponse<ProductResponseDto>
            .Success(
                response,
                "Product updated successfully."
            );
    }

    public async Task<ApiResponse<object>> DeleteAsync(
        long id,
        long userId
    )
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
}