
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.State;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    private long UserId =>
        long.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] CityFilterRequestDTO request)
    {
        var result = await _cityService.GetAllAsync(request);

        return Ok(
            ApiResponse<PagedResult<CityResponseDTO>>
                .Success(result)
        );
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _cityService.GetByIdAsync(id);

        return Ok(
            ApiResponse<CityResponseDTO>
                .Success(result)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CityRequestDTO dto)
    {
        await _cityService.CreateAsync(dto, UserId);

        return Ok(
            ApiResponse<bool>
                .Success("City created successfully.")
        );
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] CityRequestDTO dto)
    {
        await _cityService.UpdateAsync(
            id,
            dto,
            UserId);

        return Ok(
            ApiResponse<bool>
                .Success("City updated successfully.")
        );
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _cityService.DeleteAsync(
            id,
            UserId);

        return Ok(
            ApiResponse<bool>
                .Success("City deleted successfully.")
        );
    }

    [HttpGet("state/{stateId:long}")]
    public async Task<IActionResult> GetCitiesByState(
        long stateId)
    {
        var result = await _cityService
            .GetCitiesByStateAsync(stateId);

        return Ok(
            ApiResponse<List<CityResponseDTO>>
                .Success(
                    result,
                    "Cities fetched successfully."
                )
        );
    }
}

