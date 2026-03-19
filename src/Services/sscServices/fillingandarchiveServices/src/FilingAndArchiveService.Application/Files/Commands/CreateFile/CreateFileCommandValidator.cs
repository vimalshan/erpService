using FluentValidation;

namespace FilingAndArchiveService.Application.Files.Commands.CreateFile;

public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
{
    public CreateFileCommandValidator()
    {
        RuleFor(x => x.FileOrgId)
            .NotEmpty().WithMessage("Organization ID is required.")
            .MaximumLength(25).WithMessage("Organization ID cannot exceed 25 characters.");

        RuleFor(x => x.FileYear)
            .GreaterThan(0).WithMessage("File year must be a positive number.")
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 1)
                .WithMessage("File year cannot be more than one year in the future.");

        RuleFor(x => x.FileNo)
            .NotEmpty().WithMessage("File number is required.")
            .MaximumLength(25).WithMessage("File number cannot exceed 25 characters.");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("Creator user ID must be a positive number.");

        RuleFor(x => x.Remarks)
            .MaximumLength(200).WithMessage("Remarks cannot exceed 200 characters.")
            .When(x => x.Remarks != null);
    }
}
