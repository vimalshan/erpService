using FluentValidation;
using CanteenTransactionService.Application.CQRS.Commands;

namespace CanteenTransactionService.Application.Validators;

public class RecordCanteenTransactionValidator : AbstractValidator<RecordCanteenTransactionCommand>
{
    public RecordCanteenTransactionValidator()
    {
        RuleFor(x => x.CompanyCode).GreaterThan(0).WithMessage("Company code must be positive.");
        RuleFor(x => x.EmployeeSysId).GreaterThan(0).WithMessage("Employee system ID must be positive.");
        RuleFor(x => x.EmployeeType).NotEmpty().MaximumLength(1);
        RuleFor(x => x.SwipeDate).NotEmpty().WithMessage("Swipe date is required.");
        RuleFor(x => x.ItemCode).GreaterThan(0).WithMessage("Item code must be positive.");
        RuleFor(x => x.ItemType).NotEmpty().MaximumLength(1);
        RuleFor(x => x.EmployeeContribution).GreaterThanOrEqualTo(0);
        RuleFor(x => x.EmployerContribution).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ItemQuantity).GreaterThan(0).WithMessage("Item quantity must be at least 1.");
        RuleFor(x => x.EntryUser).GreaterThan(0).WithMessage("Entry user is required.");
    }
}

public class ProcessDailyAvailedValidator : AbstractValidator<ProcessDailyAvailedCommand>
{
    public ProcessDailyAvailedValidator()
    {
        RuleFor(x => x.CompanyCode).GreaterThan(0).WithMessage("Company code must be positive.");
        RuleFor(x => x.EmployeeSysId).GreaterThan(0).WithMessage("Employee system ID must be positive.");
    }
}

public class SubmitMisBatchValidator : AbstractValidator<SubmitMisBatchCommand>
{
    public SubmitMisBatchValidator()
    {
        RuleFor(x => x.CompanyCode).GreaterThan(0).WithMessage("Company code must be positive.");
        RuleFor(x => x.EmployeeNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.SwipeTime).NotEmpty().WithMessage("Swipe time is required.");
        RuleFor(x => x.ItemCode).GreaterThan(0).WithMessage("Item code must be positive.");
        RuleFor(x => x.ItemQuantity).GreaterThan(0).WithMessage("Item quantity must be at least 1.");
        RuleFor(x => x.BatchDate).NotEmpty().WithMessage("Batch date is required.");
        RuleFor(x => x.BatchNumber).GreaterThan(0).WithMessage("Batch number must be positive.");
        RuleFor(x => x.CanteenNumber).NotEmpty().MaximumLength(1);
        RuleFor(x => x.GateNumber).NotEmpty().MaximumLength(3);
    }
}
