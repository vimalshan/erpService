using FluentValidation;

namespace TdsService.Application.Files.Commands.UploadTdsFile;

public sealed class UploadTdsFileCommandValidator : AbstractValidator<UploadTdsFileCommand>
{
    public UploadTdsFileCommandValidator()
    {
        RuleFor(x => x.FileId)
            .GreaterThan(0).WithMessage("File ID must be a positive number.");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required.")
            .MaximumLength(100).WithMessage("File name must not exceed 100 characters.");

        RuleFor(x => x.PanNo)
            .Matches(@"^[A-Z]{5}[0-9]{4}[A-Z]$")
            .WithMessage("PAN number must be in the format AAAAA0000A.")
            .MaximumLength(15)
            .When(x => !string.IsNullOrWhiteSpace(x.PanNo));

        RuleFor(x => x.EmailStatus)
            .Must(s => s == null || s == "Y" || s == "N")
            .WithMessage("Email status must be 'Y' or 'N'.");

        RuleFor(x => x.FileType)
            .MaximumLength(3).WithMessage("File type must not exceed 3 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FileType));
    }
}
