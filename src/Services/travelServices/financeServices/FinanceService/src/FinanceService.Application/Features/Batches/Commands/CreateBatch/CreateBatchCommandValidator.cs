using FluentValidation;

namespace FinanceService.Application.Features.Batches.Commands.CreateBatch;

public class CreateBatchCommandValidator : AbstractValidator<CreateBatchCommand>
{
    public CreateBatchCommandValidator()
    {
        RuleFor(x => x.UnitCode)
            .NotEmpty().WithMessage("Unit code is required.")
            .MaximumLength(3).WithMessage("Unit code cannot exceed 3 characters.");

        RuleFor(x => x.AgencyCode)
            .GreaterThan(0).WithMessage("Agency code must be greater than 0.");
    }
}
