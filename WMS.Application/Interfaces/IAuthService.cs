using WMS.Application.DTOs.Auth;
using WMS.Shared.Response;

namespace WMS.Application.Interfaces;

public interface IAuthService
{   Task<SignUpResponseDto> CreateAdminAsync(SignUpRequestDto request);
    Task<SignUpResponseDto>  SignUpAsync(SignUpRequestDto request);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task LogoutAsync(string refreshToken);
}