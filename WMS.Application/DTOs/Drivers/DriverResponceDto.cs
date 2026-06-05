namespace WMS.Application.DTOs.Drivers;

public class DriverResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? LicenceNo { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }
}