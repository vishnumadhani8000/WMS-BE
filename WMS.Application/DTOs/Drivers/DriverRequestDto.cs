namespace WMS.Application.DTOs.Drivers;

public class DriverRequestDto
{
    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? LicenceNo { get; set; }

    public bool IsAvailable { get; set; } = true;
}