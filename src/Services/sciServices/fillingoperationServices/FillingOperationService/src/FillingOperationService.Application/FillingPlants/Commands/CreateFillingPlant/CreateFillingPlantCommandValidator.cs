using FluentValidation;

namespace FillingOperationService.Application.FillingPlants.Commands.CreateFillingPlant;

public class CreateFillingPlantCommandValidator : AbstractValidator<CreateFillingPlantCommand>
{
    public CreateFillingPlantCommandValidator()
    {
        RuleFor(x => x.CompanyUnitId).GreaterThan(0).WithMessage("Company unit ID is required.");
        RuleFor(x => x.PlantName).NotEmpty().MaximumLength(40).WithMessage("Plant name is required and must not exceed 40 characters.");
        RuleFor(x => x.Location).NotEmpty().MaximumLength(20).WithMessage("Location is required and must not exceed 20 characters.");
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created by user is required.");
    }
}
