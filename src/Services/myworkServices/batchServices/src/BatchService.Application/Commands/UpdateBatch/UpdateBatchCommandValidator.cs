using FluentValidation;

namespace BatchService.Application.Commands.UpdateBatch;

public sealed class UpdateBatchCommandValidator : AbstractValidator<UpdateBatchCommand>
{
    public UpdateBatchCommandValidator()
    {
        RuleFor(x => x.BatchId)
            .GreaterThan(0).WithMessage("BatchId must be positive.");

        RuleFor(x => x.MonthNo)
            .InclusiveBetween(1, 12).WithMessage("MonthNo must be between 1 and 12.");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy must be a valid user ID.");
    }
}
