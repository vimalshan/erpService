using FluentValidation;

namespace FilingAndArchiveService.Application.Files.Commands.UpdateFile;

public class UpdateFileCommandValidator : AbstractValidator<UpdateFileCommand>
{
    public UpdateFileCommandValidator()
    {
        RuleFor(x => x.FileId).GreaterThan(0).WithMessage("File ID must be a positive number.");
        RuleFor(x => x.UpdatedBy).GreaterThan(0).WithMessage("Updater user ID must be a positive number.");
        RuleFor(x => x.Remarks).MaximumLength(200).When(x => x.Remarks != null);
        RuleFor(x => x.PodNo).MaximumLength(50).When(x => x.PodNo != null);
        RuleFor(x => x.CourierName).MaximumLength(200).When(x => x.CourierName != null);
    }
}
