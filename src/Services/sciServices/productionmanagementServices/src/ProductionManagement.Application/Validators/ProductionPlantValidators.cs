using FluentValidation;
using ProductionManagement.Application.Commands.ProductionPlants;

namespace ProductionManagement.Application.Validators;

public class CreateProductionPlantValidator : AbstractValidator<CreateProductionPlantCommand>
{
    public CreateProductionPlantValidator()
    {
        RuleFor(x => x.Dto.PlantName)
            .NotEmpty().WithMessage("Plant name is required.")
            .MaximumLength(60).WithMessage("Plant name cannot exceed 60 characters.");

        RuleFor(x => x.Dto.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(25).WithMessage("Location cannot exceed 25 characters.");

        RuleFor(x => x.Dto.CompanyUnitId)
            .GreaterThan(0).WithMessage("Company unit ID must be positive.");

        RuleFor(x => x.Dto.CreatedBy)
            .GreaterThan(0).WithMessage("Created by user ID must be positive.");
    }
}

public class UpdateProductionPlantValidator : AbstractValidator<UpdateProductionPlantCommand>
{
    public UpdateProductionPlantValidator()
    {
        RuleFor(x => x.Dto.ProductionPlantId)
            .GreaterThan(0).WithMessage("Plant ID must be positive.");

        RuleFor(x => x.Dto.PlantName)
            .NotEmpty().WithMessage("Plant name is required.")
            .MaximumLength(60).WithMessage("Plant name cannot exceed 60 characters.");

        RuleFor(x => x.Dto.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(25).WithMessage("Location cannot exceed 25 characters.");
    }
}
