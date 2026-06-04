namespace WMS.Application.DTOs.Auth;

public class UpdateProfileRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string? Phone { get; set; }
}