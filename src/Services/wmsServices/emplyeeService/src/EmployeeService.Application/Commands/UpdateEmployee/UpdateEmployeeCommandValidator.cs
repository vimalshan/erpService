using FluentValidation;

namespace EmployeeService.Application.Commands.UpdateEmployee;

public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0);

        RuleFor(x => x.FirstName)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.HireDate)
            .NotEmpty();

        RuleFor(x => x.JobTitle)
            .MaximumLength(50);

        RuleFor(x => x.Department)
            .MaximumLength(50);

        RuleFor(x => x.Phone)
            .MaximumLength(20);

        RuleFor(x => x.Email)
            .MaximumLength(100)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
    }
}
