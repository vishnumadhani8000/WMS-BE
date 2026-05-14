using FluentValidation;

using WMS.Application.DTOs.Products;

namespace WMS.Application.Validators.Products;

public class UpdateProductValidator 
    : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")

            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Product name cannot contain only spaces.")

            .MaximumLength(100)
            .WithMessage("Product name must not exceed 100 characters.");



        RuleFor(x => x.WeightKg)
            .NotEmpty()
            .WithMessage("Weight is required.")

            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0.")

            .LessThanOrEqualTo(9999)
            .WithMessage("Weight must not exceed 9999 kg.");



        RuleFor(x => x.Stock)
            .NotEmpty()
            .WithMessage("Stock is required.")

            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock cannot be negative.")

            .LessThanOrEqualTo(1000000)
            .WithMessage("Stock must not exceed 1000000.");



        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.")

            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}   