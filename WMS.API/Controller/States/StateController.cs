
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.State;
using WMS.Application.Interfaces;
using WMS.Domain.Common;
using WMS.Shared.Response;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StateController : ControllerBase
{
    private readonly IStateService _stateService;

    public StateController(IStateService stateService)
    {
        _stateService = stateService;
    }

    private long UserId =>
        long.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] CommonFilterDto request)
    {
        var result = await _stateService.GetAllAsync(request);

        return Ok(
            ApiResponse<PagedResult<StateResponseDTO>>
                .Success(result, "States fetched successfully."));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _stateService.GetByIdAsync(id);

        return Ok(
            ApiResponse<StateResponseDTO>
                .Success(result, "State fetched successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] StateRequestDTO dto)
    {
        await _stateService.CreateAsync(dto, UserId);

        return Ok(
            ApiResponse<bool>
                .Success("State created successfully."));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] StateRequestDTO dto)
    {
        await _stateService.UpdateAsync(
            id,
            dto,
            UserId);

        return Ok(
            ApiResponse<bool>
                .Success("State updated successfully."));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _stateService.DeleteAsync(
            id,
            UserId);

        return Ok(
            ApiResponse<bool>
                .Success("State deleted successfully."));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllStates()
    {
        var result = await _stateService.GetAllStatesAsync();

        return Ok(
            ApiResponse<List<StateResponseDTO>>
                .Success(result, "States fetched successfully."));
    }
}

