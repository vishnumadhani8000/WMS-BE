using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Auth;
using WMS.Application.Interfaces;
using WMS.Shared.Response;

namespace WMS.API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        Response.Cookies.Append("AuthRefreshToken", result.RefreshToken!, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(30)
        });

        result.RefreshToken = null!;

        return Ok(
            ApiResponse<LoginResponseDto>.Success(
                result,
                "Logged In Successfully"
            )
        );
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var rawToken = Request.Cookies["AuthRefreshToken"];

        if (string.IsNullOrWhiteSpace(rawToken))
        {
            return Unauthorized(
                ApiResponse<string>.Failure("No refresh token found"));
        }

        var result = await _authService.RefreshTokenAsync(
            new RefreshTokenRequestDto
            {
                RefreshToken = rawToken
            });

        if (!result.IsSuccess)
        {
            return Unauthorized(result);
        }

        Response.Cookies.Append(
            "AuthRefreshToken",
            result.Data!.RefreshToken!,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(30)
            });

        result.Data.RefreshToken = null!;

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var rawToken = Request.Cookies["AuthRefreshToken"];

        if (!string.IsNullOrWhiteSpace(rawToken))
        {
            await _authService.LogoutAsync(rawToken);
        }

        Response.Cookies.Delete("AuthRefreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax
        });

        return Ok(
            ApiResponse<object>.Success(
                "Logged out successfully."
            )
        );
    }




    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto request)
    {
        var result = await _authService.SignUpAsync(request);

        return StatusCode(
            201,
            ApiResponse<SignUpResponseDto>.Success(
                result,
                "Account created successfully."
            )
        );
    }

}