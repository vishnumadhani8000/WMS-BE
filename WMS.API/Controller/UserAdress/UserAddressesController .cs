
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

    private int UserId =>
        Convert.ToInt32(
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UserAddressRequestDto dto)
    {
        await _service
            .UpdateAsync(id, UserId, dto);

        return Ok(
            ApiResponse<bool>
                .Success(true,
                    "Address updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service
            .DeleteAsync(id, UserId);

        return Ok(
            ApiResponse<bool>
                .Success(true,
                    "Address deleted successfully."));
    }
}
