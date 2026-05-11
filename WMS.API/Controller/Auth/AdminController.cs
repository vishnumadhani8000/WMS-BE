using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Auth;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]   
public class AdminController : ControllerBase
{
    private readonly IAuthService _authService;

    public AdminController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("create")]
    public async Task<IActionResult> CreateAdmin(
        [FromBody] SignUpRequestDto request)
    {
        var result = await _authService.CreateAdminAsync(request);
        return CreatedAtAction(nameof(CreateAdmin), result);
    }
}