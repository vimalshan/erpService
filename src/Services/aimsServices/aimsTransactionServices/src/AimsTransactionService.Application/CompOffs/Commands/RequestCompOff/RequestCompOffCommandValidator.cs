using FluentValidation;

namespace AimsTransactionService.Application.CompOffs.Commands.RequestCompOff;

public sealed class RequestCompOffCommandValidator : AbstractValidator<RequestCompOffCommand>
{
    public RequestCompOffCommandValidator()
    {
        RuleFor(x => x.EmployeeSysId)
            .GreaterThan(0).WithMessage("EmployeeSysId must be a valid ID.");

        RuleFor(x => x.HoursRequested)
            .GreaterThan(0).WithMessage("HoursRequested must be greater than zero.");

        RuleFor(x => x.RequestedBy)
            .GreaterThan(0).WithMessage("RequestedBy must be a valid user ID.");
    }
}
