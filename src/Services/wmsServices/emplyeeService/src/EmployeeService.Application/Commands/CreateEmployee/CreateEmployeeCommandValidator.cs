using FluentValidation;

namespace EmployeeService.Application.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);

        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("Employee code is required.")
            .MaximumLength(20);

        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("Hire date is required.");

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
