using FluentValidation;
using MedicineManagement.Application.Features.MedicineTypes.Commands;

namespace MedicineManagement.Application.Validators;

public class CreateMedicineTypeValidator : AbstractValidator<CreateMedicineTypeCommand>
{
    public CreateMedicineTypeValidator()
    {
        RuleFor(x => x.TypeCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.TypeName).MaximumLength(30);
        RuleFor(x => x.EntryUser).NotEmpty().MaximumLength(25);
    }
}
