using FluentValidation;

namespace BatchService.Application.Commands.CreateBatch;

public sealed class CreateBatchCommandValidator : AbstractValidator<CreateBatchCommand>
{
    public CreateBatchCommandValidator()
    {
        RuleFor(x => x.BatchId)
            .GreaterThan(0).WithMessage("BatchId must be a positive number.");

        RuleFor(x => x.MonthNo)
            .InclusiveBetween(1, 12).WithMessage("MonthNo must be between 1 and 12.");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy must be a valid user ID.");
    }
}
