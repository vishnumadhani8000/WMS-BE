using Microsoft.Extensions.Configuration;
using WMS.Application.DTOs.Auth;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Shared.Response;

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

    public async Task<SignUpResponseDto> SignUpAsync(SignUpRequestDto request)
    {

        var exists = await _userRepo.ExistsAsync(x => x.Email == request.Email);
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

        await _userRepo.AddAsync(user);

        return new SignUpResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepo.GetFirstOrDefaultAsync(
            x => x.Email == request.Email && x.IsActive);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await GenerateTokensAsync(user);
    }

    public async Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(
    RefreshTokenRequestDto request
    )
    {
        var existing = await _refreshTokenRepo.GetFirstOrDefaultAsync(
            x => x.Token == request.RefreshToken && !x.IsRevoked);

        if (existing == null)
        {
            return ApiResponse<LoginResponseDto>.Failure(
                "Invalid refresh token.");
        }

        if (existing.ExpiryDate < DateTime.UtcNow)
        {
            return ApiResponse<LoginResponseDto>.Failure(
                "Refresh token expired.");
        }

        existing.IsRevoked = true;

        await _refreshTokenRepo.UpdateAsync(existing);

        var user = await _userRepo.GetByIdAsync(existing.UserId);

        if (user == null || !user.IsActive)
        {
            return ApiResponse<LoginResponseDto>.Failure(
                "User not found or inactive.");
        }

        var tokens = await GenerateTokensAsync(user);

        return ApiResponse<LoginResponseDto>.Success(
            tokens,
            "Token refreshed successfully.");
    }
    public async Task LogoutAsync(string refreshToken)
    {
        var existing = await _refreshTokenRepo.GetFirstOrDefaultAsync(
            x => x.Token == refreshToken && !x.IsRevoked);

        if (existing == null) return;

        existing.IsRevoked = true;
        await _refreshTokenRepo.UpdateAsync(existing);
    }

    private async Task<LoginResponseDto> GenerateTokensAsync(User user)
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
        });

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
    public async Task<SignUpResponseDto> CreateAdminAsync(SignUpRequestDto request)
    {
        var exists = await _userRepo.ExistsAsync(x => x.Email == request.Email);
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

        await _userRepo.AddAsync(admin);

        return new SignUpResponseDto
        {
            Id = admin.Id,
            Name = admin.Name,
            Email = admin.Email,
            Role = admin.Role.ToString()
        };
    }
    public async Task<UserProfileDto> GetProfileAsync(int userId)
    {
        var user = await _userRepo.GetByIdAsync(userId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        return new UserProfileDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role.ToString(),
        };
    }
    public async Task<UserProfileDto> UpdateProfileAsync(
    int userId,
    UpdateProfileRequestDto request)
    {
        var user = await _userRepo.GetByIdAsync(userId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        user.Name = request.Name;
        user.Phone = request.Phone;

        await _userRepo.UpdateAsync(user);

        return new UserProfileDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            Role = user.Role.ToString(),
        };
    }

    public async Task ChangePasswordAsync(
    int userId,
    ChangePasswordRequestDto request)
    {
        var user = await _userRepo.GetByIdAsync(userId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        var isValidPassword =
            BCrypt.Net.BCrypt.Verify(
                request.CurrentPassword,
                user.PasswordHash);

        if (!isValidPassword)
            throw new UnauthorizedAccessException(
                "Current password is incorrect.");

        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(
                request.NewPassword);

        await _userRepo.UpdateAsync(user);
    }
}