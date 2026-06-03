    using FluentValidation;
    using WMS.Application.DTOs.Auth;

    namespace WMS.Application.Validators.Auth;

    public class UpdateProfileRequestValidator
        : AbstractValidator<UpdateProfileRequestDto>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(120)
                .WithMessage("Name must not exceed 120 characters.");

            RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")

            .Matches(@"^\+?[1-9]\d{6,13}$")
            .WithMessage("Phone number must be a valid mobile number.")

            .MaximumLength(14)
            .WithMessage("Phone number must not exceed 14 characters.");
        }
    }