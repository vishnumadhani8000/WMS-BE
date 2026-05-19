using System.Security.AccessControl;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WMS.Application.DTOs.Vehicles;
using WMS.Application.Interfaces;
using WMS.Domain.Common;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/vehicles")]
[Authorize]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _service;

    public VehicleController(IVehicleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CommonFilterDto filterDto )
    {
        var result = await _service.GetAllAsync(filterDto);

        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service.GetByIdAsync(id);

        return result.IsSuccess
            ? Ok(result)
            : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VehicleRequestDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _service.CreateAsync(
            dto,
            userId
        );

        return result.IsSuccess
            ? Ok(result)
            : BadRequest(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update( long id, VehicleRequestDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _service.UpdateAsync(
            id,
            dto,
            userId
        );

        return result.IsSuccess
            ? Ok(result)
            : NotFound(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _service.DeleteAsync(
            id,
            userId
        );

        return result.IsSuccess
            ? Ok(result)
            : NotFound(result);
    }
}