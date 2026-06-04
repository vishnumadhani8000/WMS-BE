
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WMS.Application.DTOs.UserAddresses;
using WMS.Application.Services;
using WMS.Shared.Response;

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
            User.FindFirstValue(
                ClaimTypes.NameIdentifier));

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var addresses = await _service
            .GetUserAddressesAsync(UserId);

        return Ok(
            ApiResponse<IEnumerable<UserAddressResponseDto>>
                .Success(addresses));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var address = await _service
            .GetByIdAsync(id, UserId);

        return Ok(
            ApiResponse<UserAddressResponseDto>
                .Success(address));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] UserAddressRequestDto dto)
    {
        var address = await _service
            .CreateAsync(UserId, dto);

        return Ok(
            ApiResponse<UserAddressResponseDto>
                .Success(
                    address,
                    "Address created successfully."));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UserAddressRequestDto dto)
    {
        await _service
            .UpdateAsync(id, UserId, dto);

        return Ok(
            ApiResponse<bool>
                .Success(true,
                    "Address updated successfully."));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _service
            .DeleteAsync(id, UserId);

        return Ok(
            ApiResponse<bool>
                .Success(true,
                    "Address deleted successfully."));
    }
}
