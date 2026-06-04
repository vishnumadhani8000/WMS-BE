    using FluentValidation;
    using WMS.Application.DTOs.Auth;

    namespace WMS.Application.Validators.Auth;

    public class SignUpRequestValidator : AbstractValidator<SignUpRequestDto>
    {
        public SignUpRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(120).WithMessage("Name must not exceed 120 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone number is required.")

                .Matches(@"^\+?[1-9]\d{6,13}$")
                .WithMessage("Phone number must be a valid mobile number.")

                .MaximumLength(14)
                .WithMessage("Phone number must not exceed 14 characters.");
        }
    }       