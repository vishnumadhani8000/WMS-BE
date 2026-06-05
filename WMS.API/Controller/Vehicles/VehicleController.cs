
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Vehicles;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Shared.Response;

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
    public async Task<IActionResult> GetAll([FromQuery] CommonFilterDto filterDto)
    {
        var vehicles = await _service.GetAllAsync(filterDto);

        return Ok(
            ApiResponse<PagedResult<VehicleResponseDto>>
                .Success(vehicles, "Vehicles fetched successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vehicle = await _service.GetByIdAsync(id);

        return Ok(
            ApiResponse<VehicleResponseDto>
                .Success(vehicle, "Vehicle fetched successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VehicleRequestDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var vehicle = await _service.CreateAsync(
            dto,
            userId);

        return Ok(
            ApiResponse<VehicleResponseDto>
                .Success(vehicle, "Vehicle created successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] VehicleRequestDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var vehicle = await _service.UpdateAsync(id, dto, userId);

        return Ok(
            ApiResponse<VehicleResponseDto>
                .Success(vehicle, "Vehicle updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        await _service.DeleteAsync(
            id,
            userId);

        return Ok(
            ApiResponse<object>
                .Success("Vehicle deleted successfully."));
    }
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableVehicles(
    [FromQuery] decimal totalWeightKg)
    {
        var vehicles =
            await _service.GetAvailableVehiclesAsync(totalWeightKg);

        return Ok(
            ApiResponse<List<AvailableVehicleDto>>
                .Success(
                    vehicles,
                    "Available vehicles fetched successfully."));
    }
}

