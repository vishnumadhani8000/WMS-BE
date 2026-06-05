namespace WMS.Application.DTOs.UserAddresses;

public class UserAddressResponseDto : UserAddressRequestDto
{
    public int AddressId { get; set; }
    public string StateName { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;

}