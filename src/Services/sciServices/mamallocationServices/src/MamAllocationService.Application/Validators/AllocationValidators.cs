using FluentValidation;
using MamAllocationService.Application.Commands;

namespace MamAllocationService.Application.Validators;

public class CreateAllocationDetailValidator : AbstractValidator<CreateAllocationDetailCommand>
{
    public CreateAllocationDetailValidator()
    {
        RuleFor(x => x.Allocation.AllDate).NotEmpty().WithMessage("Allocation date is required");
        RuleFor(x => x.Allocation.AllRm).GreaterThan(0).WithMessage("Raw material code must be positive");
    }
}

public class UpdateAllocationDetailValidator : AbstractValidator<UpdateAllocationDetailCommand>
{
    public UpdateAllocationDetailValidator()
    {
        RuleFor(x => x.AllDate).NotEmpty();
        RuleFor(x => x.AllRm).GreaterThan(0);
    }
}

public class CreateArrivalDetailValidator : AbstractValidator<CreateArrivalDetailCommand>
{
    public CreateArrivalDetailValidator()
    {
        RuleFor(x => x.Arrival.ArrivalQty)
            .GreaterThanOrEqualTo(0).When(x => x.Arrival.ArrivalQty.HasValue)
            .WithMessage("Arrival quantity cannot be negative");
    }
}

public class CreateConsumptionDetailValidator : AbstractValidator<CreateConsumptionDetailCommand>
{
    public CreateConsumptionDetailValidator()
    {
        RuleFor(x => x.Consumption.ConsumptionQty)
            .GreaterThanOrEqualTo(0).When(x => x.Consumption.ConsumptionQty.HasValue)
            .WithMessage("Consumption quantity cannot be negative");
    }
}

public class CreateDispatchDetailValidator : AbstractValidator<CreateDispatchDetailCommand>
{
    public CreateDispatchDetailValidator()
    {
        RuleFor(x => x.Dispatch.DispatchQty)
            .GreaterThanOrEqualTo(0).When(x => x.Dispatch.DispatchQty.HasValue)
            .WithMessage("Dispatch quantity cannot be negative");
        RuleFor(x => x.Dispatch.DispatchType)
            .MaximumLength(1).When(x => x.Dispatch.DispatchType is not null);
        RuleFor(x => x.Dispatch.DispatchInvoiceNo)
            .MaximumLength(20).When(x => x.Dispatch.DispatchInvoiceNo is not null);
    }
}
