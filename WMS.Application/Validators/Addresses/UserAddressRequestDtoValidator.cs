using FluentValidation;
using WMS.Application.DTOs.UserAddresses;

namespace WMS.Application.Validators.UserAddresses;

public class UserAddressRequestDtoValidator
    : AbstractValidator<UserAddressRequestDto>
{
    public UserAddressRequestDtoValidator()
    {
        RuleFor(x => x.StateId)
            .GreaterThan(0);

        RuleFor(x => x.CityId)  
            .GreaterThan(0);    
    
        RuleFor(x => x.AddressLine) 
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Pincode)
            .NotEmpty()
            .Length(6)
            .Matches(@"^[0-9]{6}$");

        RuleFor(x => x.Landmark)
            .MaximumLength(255);
    }
}