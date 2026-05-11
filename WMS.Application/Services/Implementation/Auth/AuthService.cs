using Microsoft.Extensions.Configuration;
using WMS.Application.DTOs.Auth;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Application.Services;

public class AuthService : IAuthService
{
    private readonly ICommonRepository<User> _userRepo;
    private readonly ICommonRepository<RefreshToken> _refreshTokenRepo;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _config;

    public AuthService(
        ICommonRepository<User> userRepo,
        ICommonRepository<RefreshToken> refreshTokenRepo,
        IJwtService jwtService,
        IConfiguration config)
    {
        _userRepo = userRepo;
        _refreshTokenRepo = refreshTokenRepo;
        _jwtService = jwtService;
        _config = config;
    }

    public async Task<SignUpResponseDto> SignUpAsync(SignUpRequestDto request, CancellationToken ct = default)
    {
        // check duplicate email
        var exists = await _userRepo.ExistsAsync(x => x.Email == request.Email, ct);
        if (exists)
            throw new InvalidOperationException("Email is already registered.");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Phone = request.Phone,
            Role = UserRole.Customer,
            IsActive = true
        };

        await _userRepo.AddAsync(user, ct);

        return new SignUpResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        var user = await _userRepo.GetFirstOrDefaultAsync(
            x => x.Email == request.Email && x.IsActive, ct);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await GenerateTokensAsync(user, ct);
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken ct = default)
    {
        var existing = await _refreshTokenRepo.GetFirstOrDefaultAsync(
            x => x.Token == request.RefreshToken && !x.IsRevoked, ct);

        if (existing == null || existing.ExpiryDate < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        existing.IsRevoked = true;
        await _refreshTokenRepo.UpdateAsync(existing, ct);

        var user = await _userRepo.GetByIdAsync(existing.UserId, ct);

        if (user == null || !user.IsActive)
            throw new UnauthorizedAccessException("User not found or inactive.");

        return await GenerateTokensAsync(user, ct);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken ct = default)
    {
        var existing = await _refreshTokenRepo.GetFirstOrDefaultAsync(
            x => x.Token == refreshToken && !x.IsRevoked, ct);

        if (existing == null) return;

        existing.IsRevoked = true;
        await _refreshTokenRepo.UpdateAsync(existing, ct);
    }

    private async Task<LoginResponseDto> GenerateTokensAsync(User user, CancellationToken ct)
    {
        var accessToken = await _jwtService.GenerateAccessToken(user.Id, user.Email, user.Role);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var expiryDays = int.Parse(_config["Jwt:RefreshTokenExpiryDays"]!);
        var expiryMinutes = int.Parse(_config["Jwt:ExpiryMinutes"]!);

        await _refreshTokenRepo.AddAsync(new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(expiryDays),
            IsRevoked = false
        }, ct);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
    public async Task<SignUpResponseDto> CreateAdminAsync(SignUpRequestDto request, CancellationToken ct = default)
    {
        var exists = await _userRepo.ExistsAsync(x => x.Email == request.Email, ct);
        if (exists)
            throw new InvalidOperationException("Email is already registered.");

        var admin = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Phone = request.Phone,
            Role = UserRole.Admin,
            IsActive = true
        };

        await _userRepo.AddAsync(admin, ct);

        return new SignUpResponseDto
        {
            Id = admin.Id,
            Name = admin.Name,
            Email = admin.Email,
            Role = admin.Role.ToString()
        };
    }
}