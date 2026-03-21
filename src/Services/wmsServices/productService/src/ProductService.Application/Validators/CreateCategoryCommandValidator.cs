using FluentValidation;
using ProductService.Application.Commands.CreateCategory;

namespace ProductService.Application.Validators;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Dto.CategoryName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.Description).MaximumLength(255);
    }
}
