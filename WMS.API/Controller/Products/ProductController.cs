using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Products;
using WMS.Application.Interfaces;
using WMS.Domain.Common;

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

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductRequestDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result =
            await _service.CreateAsync(dto, userId);

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id,ProductRequestDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        var result =
            await _service.UpdateAsync(id,dto, userId);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var userIdClaim =  User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result =await _service.DeleteAsync(id, userId);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}