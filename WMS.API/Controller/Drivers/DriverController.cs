using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Drivers;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/drivers")]
[Authorize]
public class DriverController : ControllerBase
{
    private readonly IDriverService _service;

    public DriverController(IDriverService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] CommonFilterDto filterDto)
    {
        var drivers = await _service.GetAllAsync(filterDto);

        return Ok(
            ApiResponse<PagedResult<DriverResponseDto>>
                .Success(
                    drivers,
                    "Drivers fetched successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var driver = await _service.GetByIdAsync(id);

        return Ok(
            ApiResponse<DriverResponseDto>
                .Success(
                    driver,
                    "Driver fetched successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] DriverRequestDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var driver = await _service.CreateAsync(
            dto,
            userId);

        return Ok(
            ApiResponse<DriverResponseDto>
                .Success(
                    driver,
                    "Driver created successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] DriverRequestDto dto)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var driver = await _service.UpdateAsync(
            id,
            dto,
            userId);

        return Ok(
            ApiResponse<DriverResponseDto>
                .Success(
                    driver,
                    "Driver updated successfully."));
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

        await _service.DeleteAsync(
            id,
            userId);

        return Ok(
            ApiResponse<object>
                .Success(
                    "Driver deleted successfully."));
    }
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableDrivers()
    {
        var drivers =
            await _service.GetAvailableDriversAsync();

        return Ok(
            ApiResponse<List<AvailableDriversDto>>
                .Success(
                    drivers,
                    "Available drivers fetched successfully."));
    }
}