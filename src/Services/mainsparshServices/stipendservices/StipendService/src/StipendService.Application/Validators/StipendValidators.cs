using FluentValidation;
using StipendService.Application.Features.StipendMaster.Commands;
using StipendService.Application.Features.StipendDisbursement.Commands;

namespace StipendService.Application.Validators;

public class CreateStipendMasterValidator : AbstractValidator<CreateStipendMasterCommand>
{
    public CreateStipendMasterValidator()
    {
        RuleFor(x => x.ResearchCategoryId).GreaterThan(0).WithMessage("ResearchCategoryId must be positive.");
        RuleFor(x => x.SrfRankId).GreaterThan(0).WithMessage("SrfRankId must be positive.");
        RuleFor(x => x.SrfMonthlyStipend).GreaterThanOrEqualTo(0).WithMessage("Monthly stipend cannot be negative.");
        RuleFor(x => x.EffectiveFrom).NotEmpty().WithMessage("EffectiveFrom is required.");
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("CreatedBy must be positive.");
        RuleFor(x => x.EffectiveTo)
            .Must((cmd, to) => to == null || to > cmd.EffectiveFrom)
            .WithMessage("EffectiveTo must be after EffectiveFrom.");
    }
}

public class UpdateStipendMasterValidator : AbstractValidator<UpdateStipendMasterCommand>
{
    public UpdateStipendMasterValidator()
    {
        RuleFor(x => x.StipendId).GreaterThan(0);
        RuleFor(x => x.SrfMonthlyStipend).GreaterThanOrEqualTo(0);
        RuleFor(x => x.EffectiveFrom).NotEmpty();
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class ProcessMonthlyStipendValidator : AbstractValidator<ProcessMonthlyStipendCommand>
{
    public ProcessMonthlyStipendValidator()
    {
        RuleFor(x => x.MonthYear)
            .NotEmpty()
            .Matches(@"^\d{4}-(0[1-9]|1[0-2])$")
            .WithMessage("MonthYear must be in format YYYY-MM.");
        RuleFor(x => x.ProcessedBy).GreaterThan(0);
    }
}

public class CalculateAndDisburseValidator : AbstractValidator<CalculateAndDisburseStipendCommand>
{
    public CalculateAndDisburseValidator()
    {
        RuleFor(x => x.MonthYear)
            .NotEmpty()
            .Matches(@"^\d{4}-(0[1-9]|1[0-2])$")
            .WithMessage("MonthYear must be in format YYYY-MM.");
        RuleFor(x => x.ProcessedBy).GreaterThan(0);
    }
}
