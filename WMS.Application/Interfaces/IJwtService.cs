using WMS.Domain.Enums;

namespace WMS.Application.Interfaces;

public interface IJwtService
{
    Task<string> GenerateAccessToken(int id, string email, UserRole role);
    string GenerateRefreshToken();
}