using FluentValidation;
using MedicineManagement.Application.Features.Medicines.Commands;

namespace MedicineManagement.Application.Validators;

public class CreateMedicineValidator : AbstractValidator<CreateMedicineCommand>
{
    public CreateMedicineValidator()
    {
        RuleFor(x => x.MedicineCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.MedicineName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.MedicineTypeCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Category).Must(c => c is null or 'H' or 'M' or 'L')
            .WithMessage("Category must be H, M, or L.");
        RuleFor(x => x.EntryUser).NotEmpty().MaximumLength(25);
    }
}
