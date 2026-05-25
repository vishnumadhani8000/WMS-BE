using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS.Application.DTOs.UserAddresses;
using WMS.Application.Services;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/user-addresses")]
[Authorize]
public class UserAddressesController : ControllerBase
{
    private readonly IUserAddressService _service;

    public UserAddressesController(
        IUserAddressService service)
    {
        _service = service;
    }

    private long UserId =>
        Convert.ToInt64(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service
            .GetUserAddressesAsync(UserId);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _service
            .GetByIdAsync(id, UserId);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        UserAddressRequestDto dto)
    {
        var result = await _service
            .CreateAsync(UserId, dto);

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        UserAddressRequestDto dto)
    {
        var result = await _service
            .UpdateAsync(id, UserId, dto);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _service
            .DeleteAsync(id, UserId);

        return Ok(result);
    }
}