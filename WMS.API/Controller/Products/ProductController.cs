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
    public async Task<IActionResult> GetAll([FromQuery] CommonFilterDto filterDto)
    {
        var result = await _service.GetAllAsync(filterDto);

        return Ok(
            ApiResponse<PagedResult<BaseProductDto>>
                .Success(result, "Products fetched successfully.")
        );
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        return Ok(
            ApiResponse<BaseProductDto>
                .Success(result, "Product fetched successfully.")
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(BaseProductDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _service.CreateAsync(dto, userId);

        return Ok(
            ApiResponse<BaseProductDto>
                .Success(result, "Product created successfully.")
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(BaseProductDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _service.UpdateAsync(dto, userId);

        return Ok(
            ApiResponse<BaseProductDto>
                .Success(result, "Product updated successfully.")
        );
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        await _service.DeleteAsync(id, userId);

        return Ok(
            ApiResponse<string>
                .Success("Product deleted successfully.")
        );
    }

    [HttpGet("customer")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllForCustomer(
        [FromQuery] CommonFilterDto filterDto)
    {
        var result =
            await _service.GetAllForCustomerAsync(filterDto);

        return Ok(
            ApiResponse<PagedResult<ProductResponseCustomerDto>>
                .Success(result, "Products fetched successfully.")
        );
    }
}