using FluentValidation;

namespace ComplaintService.Application.Commands.ReopenComplaint;

public sealed class ReopenComplaintCommandValidator : AbstractValidator<ReopenComplaintCommand>
{
    public ReopenComplaintCommandValidator()
    {
        RuleFor(x => x.TicketNum).GreaterThan(0);
        RuleFor(x => x.Remarks).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.ReopenedBy).GreaterThan(0);
    }
}
