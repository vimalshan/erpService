using FluentValidation;

namespace ComplaintService.Application.Commands.CloseComplaint;

public sealed class CloseComplaintCommandValidator : AbstractValidator<CloseComplaintCommand>
{
    public CloseComplaintCommandValidator()
    {
        RuleFor(x => x.TicketNum).GreaterThan(0);
        RuleFor(x => x.FinalRemarks).MaximumLength(500).When(x => x.FinalRemarks != null);
        RuleFor(x => x.ClosedBy).GreaterThan(0);
    }
}
