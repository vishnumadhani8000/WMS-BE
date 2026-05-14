using FluentValidation;
using WMS.Application.DTOs.Vehicles;

namespace WMS.Application.Validators.Vehicles;

public class VehicleValidator
    : AbstractValidator<VehicleRequestDto>
{
    public VehicleValidator()
    {
        // ─────────────────────────────────────────
        // Vehicle Name
        // ─────────────────────────────────────────
        RuleFor(x => x.Name)

            .NotEmpty()
            .WithMessage("Vehicle name is required.")

            .Must(name =>
                !string.IsNullOrWhiteSpace(name)
            )
            .WithMessage(
                "Vehicle name cannot contain only spaces."
            )

            .MinimumLength(2)
            .WithMessage(
                "Vehicle name must be at least 2 characters."
            )

            .MaximumLength(100)
            .WithMessage(
                "Vehicle name must not exceed 100 characters."
            );



        // ─────────────────────────────────────────
        // Plate Number
        // ─────────────────────────────────────────
        RuleFor(x => x.PlateNumber)

            .NotEmpty()
            .WithMessage("Plate number is required.")

            .Must(plate =>
            {
                if (string.IsNullOrWhiteSpace(plate))
                    return false;

                // Normalize input
                plate = plate
                    .Trim()
                    .Replace(" ", "")
                    .Replace("-", "")
                    .ToUpper();

                // Indian Standard + Bharat Series
                return System.Text.RegularExpressions.Regex.IsMatch(
                    plate,
                    @"^([A-Z]{2}[0-9]{2}[A-Z]{1,2}[0-9]{4}|[0-9]{2}BH[0-9]{4}[A-Z]{2})$"
                );
            })
            .WithMessage(
                "Invalid Indian vehicle plate number format."
            );



        // ─────────────────────────────────────────
        // Capacity
        // ─────────────────────────────────────────
        RuleFor(x => x.CapacityKg)

            .NotEmpty()
            .WithMessage("Capacity is required.")

            .GreaterThan(0)
            .WithMessage(
                "Capacity must be greater than 0."
            )

            .LessThanOrEqualTo(100000)
            .WithMessage(
                "Capacity must not exceed 100000 kg."
            );



        // ─────────────────────────────────────────
        // Availability
        // ─────────────────────────────────────────
        RuleFor(x => x.IsAvailable)

            .NotNull()
            .WithMessage(
                "Availability status is required."
            );
    }
}