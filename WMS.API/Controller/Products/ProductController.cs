
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Products;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null,
    [FromQuery] string? sortBy = null,
    [FromQuery] bool ascending = true)
    {
        var result = await _service.GetAllAsync(
            pageNumber,
            pageSize,
            search,
            sortBy,
            ascending
        );

        return Ok(
            ApiResponse<PagedResult<ProductResponseDto>>
                .Success(result, "Products fetched successfully.")
        );
    }


    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound(
                ApiResponse<ProductResponseDto>
                .Failure("Product not found.")
            );
        }
        return Ok(ApiResponse<ProductResponseDto>.Success(product, "Product fetched successfully."));
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        var product = await _service.CreateAsync(dto, userId);

        return Ok(ApiResponse<ProductResponseDto>.Success(product, "Product created successfully."));
    }



    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, UpdateProductDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        var updated = await _service.UpdateAsync(id, dto, userId);

        if (!updated)
        {
            return NotFound(ApiResponse<object>.Failure("Product not found."));
        }

        return Ok(ApiResponse<object>.Success("Product updated successfully."));
    }


    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        var deleted = await _service.DeleteAsync(id, userId);

        if (!deleted)
        {
            return NotFound(ApiResponse<object>.Failure("Product not found."));
        }

        return Ok(ApiResponse<object>.Success("Product deleted successfully."));
    }
}