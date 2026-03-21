using FluentValidation;

namespace FinanceService.Application.Features.Payments.Commands.ProcessPayment;

public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
{
    private static readonly string[] ValidModes = { "CHQ", "BNK", "CSH" };

    public ProcessPaymentCommandValidator()
    {
        RuleFor(x => x.BatchNumber)
            .GreaterThan(0).WithMessage("Batch number must be greater than 0.");

        RuleFor(x => x.PaymentAmount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than 0.");

        RuleFor(x => x.PaymentMode)
            .NotEmpty().WithMessage("Payment mode is required.")
            .Must(m => ValidModes.Contains(m)).WithMessage("Payment mode must be CHQ, BNK, or CSH.");
    }
}
