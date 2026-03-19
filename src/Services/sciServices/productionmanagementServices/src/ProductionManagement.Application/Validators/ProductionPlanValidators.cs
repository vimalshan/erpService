using FluentValidation;
using ProductionManagement.Application.Commands.ProductionPlans;

namespace ProductionManagement.Application.Validators;

public class CreateProductionPlanValidator : AbstractValidator<CreateProductionPlanCommand>
{
    public CreateProductionPlanValidator()
    {
        RuleFor(x => x.Dto.ProductionPlantId)
            .GreaterThan(0).WithMessage("Plant ID must be positive.");

        RuleFor(x => x.Dto.SciItemId)
            .GreaterThan(0).WithMessage("Item ID must be positive.");

        RuleFor(x => x.Dto.QtyPerDay)
            .GreaterThan(0).WithMessage("Quantity per day must be positive.");
    }
}
