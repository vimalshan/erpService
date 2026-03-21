using FluentValidation;
using ProductService.Application.Commands.CreateProduct;

namespace ProductService.Application.Validators;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Dto.Sku).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.UnitOfMeasure).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Dto.Price).GreaterThanOrEqualTo(0).When(x => x.Dto.Price.HasValue);
        RuleFor(x => x.Dto.WeightPerUnit).GreaterThanOrEqualTo(0).When(x => x.Dto.WeightPerUnit.HasValue);
        RuleFor(x => x.Dto.VolumePerUnit).GreaterThanOrEqualTo(0).When(x => x.Dto.VolumePerUnit.HasValue);
    }
}
