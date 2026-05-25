namespace WMS.Application.DTOs.UserAddresses;

public class UserAddressRequestDto
{
    public long StateId { get; set; }

    public long CityId { get; set; }

    public string AddressLine { get; set; } = string.Empty;

    public string? Landmark { get; set; }

    public string Pincode { get; set; } = string.Empty;

}