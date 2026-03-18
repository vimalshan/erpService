using FluentValidation;

namespace InventoryManagement.Application.Commands.Products;

public sealed class RegisterProductCommandValidator : AbstractValidator<RegisterProductCommand>
{
    public RegisterProductCommandValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(20).WithMessage("Product name cannot exceed 20 characters.");

        RuleFor(x => x.ProductDescription)
            .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");

        RuleFor(x => x.UnitId).GreaterThan(0).WithMessage("Valid unit is required.");
        RuleFor(x => x.ProductTypeId).GreaterThan(0).WithMessage("Valid product type is required.");
        RuleFor(x => x.CompanyUnitId).GreaterThan(0).WithMessage("Valid company unit is required.");
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("CreatedBy user is required.");
    }
}
