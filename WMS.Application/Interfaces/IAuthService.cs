using WMS.Application.DTOs.Auth;

namespace WMS.Application.Interfaces;

public interface IAuthService
{   Task<SignUpResponseDto> CreateAdminAsync(SignUpRequestDto request, CancellationToken ct = default);
    Task<SignUpResponseDto>  SignUpAsync(SignUpRequestDto request, CancellationToken ct = default);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
    Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken ct = default);
    Task LogoutAsync(string refreshToken, CancellationToken ct = default);
}