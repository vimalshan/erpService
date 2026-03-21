using FluentValidation;
using TaskServices.Application.Features.TaskMails.Commands;

namespace TaskServices.Application.Features.TaskMails.Validators;

public class UpdateTaskMailCommandValidator : AbstractValidator<UpdateTaskMailCommand>
{
    public UpdateTaskMailCommandValidator()
    {
        RuleFor(x => x.MID)
            .GreaterThan(0).WithMessage("MID must be a positive number.");

        RuleFor(x => x.SYSID)
            .GreaterThan(0).WithMessage("SYSID must be a positive number.");
    }
}
