using FluentValidation;

namespace FilingAndArchiveService.Application.Files.Commands.DispatchFile;

public class DispatchFileCommandValidator : AbstractValidator<DispatchFileCommand>
{
    public DispatchFileCommandValidator()
    {
        RuleFor(x => x.FileId).GreaterThan(0);
        RuleFor(x => x.PodNo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CourierName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DispatchedBy).GreaterThan(0);
    }
}
