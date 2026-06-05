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

    public async Task<PagedResult<BaseProductDto>> GetAllAsync(CommonFilterDto requestDto)
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

            return result;
    }

    public async Task<BaseProductDto> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        return _mapper.Map<BaseProductDto>(product);
    }
    public async Task<BaseProductDto> CreateAsync(BaseProductDto dto, int userId)
    {
        var product = _mapper.Map<Product>(dto);

        product.CreatedBy = userId;

        await _repository.AddAsync(product);

        return _mapper.Map<BaseProductDto>(product);
    }

    public async Task<BaseProductDto> UpdateAsync(BaseProductDto dto, int userId)
    {
        if (!dto.Id.HasValue)
        {
            throw new ArgumentException("Product id is required.");
        }

        var product = await _repository.GetByIdAsync(dto.Id.Value);

        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        _mapper.Map(dto, product);

        product.UpdatedBy = userId;
        product.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(product);

        return _mapper.Map<BaseProductDto>(product);
    }

    public async Task DeleteAsync(int id, int userId)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        product.DeletedBy = userId;
        product.DeletedAt = DateTime.UtcNow;

        await _repository.SoftDeleteAsync(product);
    }
    public async Task<PagedResult<ProductResponseCustomerDto>> GetAllForCustomerAsync(CommonFilterDto requestDto)
    {
        IQueryable<Product> query = _repository.Query();

        query = query.Where(x => x.Stock > 0);

        if (!string.IsNullOrWhiteSpace(requestDto.Search))
        {
            requestDto.Search = requestDto.Search.Trim().ToLower();

            var search = requestDto.Search.ToLower();

            query = query.Where(x =>
                x.Name.ToLower().Contains(search) ||
                (x.Description != null &&
                 x.Description.ToLower().Contains(search))
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
            _mapper.Map<List<ProductResponseCustomerDto>>(products);

        var result = new PagedResult<ProductResponseCustomerDto>
        {
            Items = productDtos,
            TotalCount = totalCount,
            PageNumber = requestDto.PageNumber,
            PageSize = requestDto.PageSize,
        };

        return result;
    }
}