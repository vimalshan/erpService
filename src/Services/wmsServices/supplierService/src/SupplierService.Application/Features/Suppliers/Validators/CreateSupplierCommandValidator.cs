using FluentValidation;
using SupplierService.Application.Features.Suppliers.Commands;

namespace SupplierService.Application.Features.Suppliers.Validators;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Supplier.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(30).WithMessage("Code must not exceed 30 characters.");

        RuleFor(x => x.Supplier.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Supplier.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Supplier.Email))
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

        RuleFor(x => x.Supplier.Phone)
            .MaximumLength(30).WithMessage("Phone must not exceed 30 characters.");

        RuleFor(x => x.Supplier.ContactPerson)
            .MaximumLength(100).WithMessage("Contact person must not exceed 100 characters.");
    }
}
