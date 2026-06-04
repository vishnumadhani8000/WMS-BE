using FluentValidation;

public class MakeShipmentRequestValidator
    : AbstractValidator<MakeShipmentRequestDto>
{
    public MakeShipmentRequestValidator()
    {
        RuleFor(x => x.DriverId)
            .GreaterThan(0);

        RuleFor(x => x.VehicleId)
            .GreaterThan(0);

        RuleFor(x => x.OrderIds)
            .NotEmpty()
            .WithMessage("At least one order must be selected.");

        RuleForEach(x => x.OrderIds)
            .GreaterThan(0);
    }
}