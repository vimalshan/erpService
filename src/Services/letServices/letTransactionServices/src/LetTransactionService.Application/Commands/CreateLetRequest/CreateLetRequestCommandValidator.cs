using FluentValidation;

namespace LetTransactionService.Application.Commands.CreateLetRequest;

public class CreateLetRequestCommandValidator : AbstractValidator<CreateLetRequestCommand>
{
    public CreateLetRequestCommandValidator()
    {
        RuleFor(x => x.RequestNumber).GreaterThan(0).WithMessage("Request number must be positive.");
        RuleFor(x => x.FinancialYearSerialNo).GreaterThan(0).WithMessage("Financial year serial must be positive.");
        RuleFor(x => x.EmployeeUserId).NotEmpty().MaximumLength(25).WithMessage("Employee user ID is required.");
        RuleFor(x => x.SupervisorUserId).MaximumLength(25);
    }
}
