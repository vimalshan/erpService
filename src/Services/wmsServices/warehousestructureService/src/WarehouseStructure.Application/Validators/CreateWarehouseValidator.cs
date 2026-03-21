using FluentValidation;
using WarehouseStructure.Application.Commands.CreateWarehouse;

namespace WarehouseStructure.Application.Validators;

public sealed class CreateWarehouseValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseValidator()
    {
        RuleFor(x => x.Dto.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(20).WithMessage("Code must not exceed 20 characters.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Dto.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Dto.Email))
            .WithMessage("A valid email address is required.");

        RuleFor(x => x.Dto.Phone)
            .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.Dto.Phone));
    }
}
