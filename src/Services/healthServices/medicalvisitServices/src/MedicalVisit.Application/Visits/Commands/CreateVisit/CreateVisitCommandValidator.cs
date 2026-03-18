using FluentValidation;

namespace MedicalVisit.Application.Visits.Commands.CreateVisit;

public class CreateVisitCommandValidator : AbstractValidator<CreateVisitCommand>
{
    public CreateVisitCommandValidator()
    {
        RuleFor(v => v.CompanyCode)
            .NotEmpty().WithMessage("Company code is required")
            .MaximumLength(3).WithMessage("Company code cannot exceed 3 characters");

        RuleFor(v => v.MedicalUserId)
            .NotEmpty().WithMessage("Medical user ID is required")
            .MaximumLength(25).WithMessage("Medical user ID cannot exceed 25 characters");

        RuleFor(v => v.DoctorCode)
            .NotEmpty().WithMessage("Doctor code is required")
            .MaximumLength(10).WithMessage("Doctor code cannot exceed 10 characters");

        RuleFor(v => v.PatientDiagnosis)
            .NotEmpty().WithMessage("Patient diagnosis is required")
            .MaximumLength(200).WithMessage("Patient diagnosis cannot exceed 200 characters");

        RuleFor(v => v.TreatmentRemarks)
            .NotEmpty().WithMessage("Treatment remarks are required")
            .MaximumLength(200).WithMessage("Treatment remarks cannot exceed 200 characters");

        RuleFor(v => v.DoctorRemarks)
            .MaximumLength(1000).WithMessage("Doctor remarks cannot exceed 1000 characters");

        RuleFor(v => v.VisitDate)
            .NotEmpty().WithMessage("Visit date is required");

        RuleFor(v => v.CreatedBy)
            .NotEmpty().WithMessage("Created by is required");
    }
}
