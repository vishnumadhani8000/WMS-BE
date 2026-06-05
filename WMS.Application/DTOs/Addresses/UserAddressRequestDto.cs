namespace WMS.Application.DTOs.UserAddresses;

public class UserAddressRequestDto
{
    public int StateId { get; set; }

    public int CityId { get; set; }

    public string AddressLine { get; set; } = string.Empty;

    public string? Landmark { get; set; }

    public string Pincode { get; set; } = string.Empty;

}