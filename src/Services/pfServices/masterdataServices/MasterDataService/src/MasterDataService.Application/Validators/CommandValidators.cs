using FluentValidation;
using MasterDataService.Application.Features.LovMaster.Commands;
using MasterDataService.Application.Features.Configuration.Commands;

namespace MasterDataService.Application.Validators;

public class CreateLovCommandValidator : AbstractValidator<CreateLovCommand>
{
    public CreateLovCommandValidator()
    {
        RuleFor(x => x.LovCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.LovDescription).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LovValue).NotEmpty().MaximumLength(20);
        RuleFor(x => x.LovCategory).NotEmpty().MaximumLength(50);
    }
}

public class CreateConfigurationCommandValidator : AbstractValidator<CreateConfigurationCommand>
{
    public CreateConfigurationCommandValidator()
    {
        RuleFor(x => x.ConfigKey).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ConfigValue).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ConfigType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
