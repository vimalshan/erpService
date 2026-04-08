using EximManagement.Application.Commands.DataFiles;
using EximManagement.Application.Commands.Products;
using FluentValidation;

namespace EximManagement.Application.Validators;

public class CreateDataFileCommandValidator : AbstractValidator<CreateDataFileCommand>
{
    private static readonly string[] AllowedTypes = ["IMPORT", "EXPORT"];

    public CreateDataFileCommandValidator()
    {
        RuleFor(x => x.FileId).GreaterThan(0);
        RuleFor(x => x.FileType).NotEmpty().Must(t => AllowedTypes.Contains(t.ToUpperInvariant()))
            .WithMessage("FileType must be IMPORT or EXPORT.");
        RuleFor(x => x.FileName).MaximumLength(200);
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.ProductName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0);
        RuleFor(x => x.ProductName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}
