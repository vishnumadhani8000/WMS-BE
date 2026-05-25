using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.State;
using WMS.Application.Interfaces;
using WMS.Domain.Common;

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


    [HttpGet]
    public async Task<IActionResult> GetAll(
      [FromQuery] CommonFilterDto request)
    {
        var result = await _stateService.GetAllAsync(request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }


    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _stateService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StateRequestDTO dto)
    {

        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _stateService.CreateAsync(dto, userId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] StateRequestDTO dto)
    {

        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _stateService.UpdateAsync(id, dto, userId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        var result = await _stateService.DeleteAsync(id, userId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
    [HttpGet ("all")]
    public async Task<IActionResult> GetAllStateAsynce(){
        var result = await _stateService.GetAllStatesAsync();
        return Ok(result);
    }
}