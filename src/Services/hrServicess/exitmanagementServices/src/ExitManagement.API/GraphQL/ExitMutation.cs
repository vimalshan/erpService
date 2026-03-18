using ExitManagement.Application.Features.EmployeeExits.Commands;
using ExitManagement.Application.Features.ExitInterviews.Commands;
using MediatR;

namespace ExitManagement.API.GraphQL;

public class ExitMutation
{
    public async Task<decimal> CreateExitAsync(CreateExitCommand input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> ApproveExitAsync(decimal exitNo, decimal approvedBy, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new ApproveExitCommand(exitNo, approvedBy), ct);

    public async Task<bool> RevokeExitAsync(decimal exitNo, string reason, decimal revokedBy, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new RevokeExitCommand(exitNo, reason, revokedBy), ct);

    public async Task<bool> SubmitInterviewFeedbackAsync(SubmitInterviewFeedbackCommand input, [Service] IMediator mediator, CancellationToken ct)
    {
        await mediator.Send(input, ct);
        return true;
    }
}
