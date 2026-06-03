using FluentValidation;
using WMS.Application.DTOs.Drivers;

namespace WMS.Application.Validators.Drivers;

public class DriverValidator
    : AbstractValidator<DriverRequestDto>
{
    public DriverValidator()
    {
        RuleFor(x => x.Name)

            .NotEmpty()
            .WithMessage("Driver name is required.")

            .Must(name =>
                !string.IsNullOrWhiteSpace(name))
            .WithMessage(
                "Driver name cannot contain only spaces.")

            .MinimumLength(2)
            .WithMessage(
                "Driver name must be at least 2 characters.")

            .MaximumLength(100)
            .WithMessage(
                "Driver name must not exceed 100 characters.");


        RuleFor(x => x.Phone)
           .NotEmpty()
           .WithMessage("Phone number is required.")

           .Matches(@"^\+?[1-9]\d{6,13}$")
           .WithMessage("Phone number must be a valid mobile number.")

           .MaximumLength(14)
           .WithMessage("Phone number must not exceed 14 characters.");

        RuleFor(x => x.LicenceNo)
        .NotEmpty()
        .WithMessage("Licence number is required.")

        .Matches(@"^[A-Z]{2}[0-9]{2}\s?[0-9]{4}[0-9]{7}$")
        .WithMessage("Invalid licence number format.")

        .MaximumLength(50)
        .WithMessage("Licence number must not exceed 50 characters.");


        RuleFor(x => x.IsAvailable)

            .NotNull()
            .WithMessage(
                "Availability status is required.");
    }
}