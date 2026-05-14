using AutoMapper;
using Microsoft.EntityFrameworkCore;

using WMS.Application.DTOs.Products;
using WMS.Application.Interfaces;

using WMS.Domain.Common;
using WMS.Domain.Entities;

namespace WMS.Application.Services;

public class ProductService : IProductService
{
    private readonly ICommonRepository<Product> _repository;

    private readonly IMapper _mapper;

    public ProductService(ICommonRepository<Product> repository, IMapper mapper
)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductResponseDto>> GetAllAsync(
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

        // ── AutoMapper ─────────────────────────
        var productDtos = _mapper.Map<List<ProductResponseDto>>(products);

        return new PagedResult<ProductResponseDto>
        {
            Items = productDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
    }

    public async Task<ProductResponseDto?> GetByIdAsync(long id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return null;
        }

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto> CreateAsync(      
        CreateProductDto dto,
        long userId
    )
    {
        var product = _mapper.Map<Product>(dto);
        product.CreatedBy = userId;
        await _repository.AddAsync(product);
        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<bool> UpdateAsync(
        long id,
        UpdateProductDto dto,
        long userId
    )
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        // Update existing entity
        _mapper.Map(dto, product);
        product.UpdatedBy = userId;
        await _repository.UpdateAsync(product);

        return true;
    }

    public async Task<bool> DeleteAsync(
        long id,
        long userId
    )
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            return false;
        }

        product.DeletedBy = userId;
        await _repository.SoftDeleteAsync(product);
        return true;
    }
}