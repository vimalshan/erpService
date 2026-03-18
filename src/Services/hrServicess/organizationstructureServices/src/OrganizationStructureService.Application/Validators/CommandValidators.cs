using FluentValidation;
using OrganizationStructureService.Application.Commands;

namespace OrganizationStructureService.Application.Validators;

public class CreateBusinessCommandValidator : AbstractValidator<CreateBusinessCommand>
{
    public CreateBusinessCommandValidator()
    {
        RuleFor(x => x.BusinessId).GreaterThan(0).WithMessage("BusinessId must be positive.");
        RuleFor(x => x.BusinessName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BusinessShortName).NotEmpty().MaximumLength(10);
        RuleFor(x => x.BusinessCode).NotEmpty().MaximumLength(9);
        RuleFor(x => x.CompanyId).GreaterThan(0);
        RuleFor(x => x.CompanyCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class UpdateBusinessCommandValidator : AbstractValidator<UpdateBusinessCommand>
{
    public UpdateBusinessCommandValidator()
    {
        RuleFor(x => x.BusinessId).GreaterThan(0);
        RuleFor(x => x.BusinessName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.BusinessShortName).NotEmpty().MaximumLength(10);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class CreateUnitCommandValidator : AbstractValidator<CreateUnitCommand>
{
    public CreateUnitCommandValidator()
    {
        RuleFor(x => x.UnitId).GreaterThan(0);
        RuleFor(x => x.UnitName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UnitShortName).NotEmpty().MaximumLength(20);
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.BusinessId).GreaterThan(0);
        RuleFor(x => x.BusinessCode).NotEmpty().MaximumLength(9);
        RuleFor(x => x.OrgId).GreaterThan(0);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class CreateGradeCommandValidator : AbstractValidator<CreateGradeCommand>
{
    public CreateGradeCommandValidator()
    {
        RuleFor(x => x.GradeId).GreaterThan(0);
        RuleFor(x => x.GradeName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.GradeCode).MaximumLength(3).When(x => x.GradeCode != null);
        RuleFor(x => x.GradeDesignation).MaximumLength(50).When(x => x.GradeDesignation != null);
    }
}

public class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
{
    public CreatePositionCommandValidator()
    {
        RuleFor(x => x.PositionId).GreaterThan(0);
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.GradeId).GreaterThan(0);
        RuleFor(x => x.Designation).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EffectiveDate).LessThanOrEqualTo(DateTime.Today.AddYears(10));
        RuleFor(x => x.ReferenceCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.EnteredBy).GreaterThan(0);
    }
}
