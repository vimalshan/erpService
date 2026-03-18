using FluentValidation;

namespace FillingOperationService.Application.FillingLines.Commands.CreateFillingLine;

public class CreateFillingLineCommandValidator : AbstractValidator<CreateFillingLineCommand>
{
    public CreateFillingLineCommandValidator()
    {
        RuleFor(x => x.FillingPlantId).GreaterThan(0).WithMessage("Filling plant ID is required.");
        RuleFor(x => x.FillingLineName).NotEmpty().MaximumLength(30).WithMessage("Filling line name is required.");
        RuleFor(x => x.NoOfFillingPoints).GreaterThan(0).WithMessage("Number of filling points must be positive.");
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created by user is required.");
    }
}
