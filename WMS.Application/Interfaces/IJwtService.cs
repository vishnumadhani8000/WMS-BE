using WMS.Domain.Enums;

namespace WMS.Application.Interfaces;

public interface IJwtService
{
    Task<string> GenerateAccessToken(long id, string email, UserRole role);
    string GenerateRefreshToken();
}