using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.State;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;

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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CityFilterRequestDTO request)
    {
        var result = await _cityService.GetAllAsync(request);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

  
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _cityService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CityRequestDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _cityService.CreateAsync(dto, userId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

   
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] CityRequestDTO dto)
    {   
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            

        var result = await _cityService.UpdateAsync(id, dto, userId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {   var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        var result = await _cityService.DeleteAsync(id, userId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}