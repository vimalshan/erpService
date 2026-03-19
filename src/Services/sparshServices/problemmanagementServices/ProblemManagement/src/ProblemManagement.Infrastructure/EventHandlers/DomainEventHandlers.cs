using MediatR;
using Microsoft.Extensions.Logging;
using ProblemManagement.Domain.Events;
using ProblemManagement.Domain.Interfaces;

namespace ProblemManagement.Infrastructure.EventHandlers;

public class ProblemCreatedEventHandler(
    IMessagePublisher publisher,
    ILogger<ProblemCreatedEventHandler> logger) : INotificationHandler<ProblemCreatedEvent>
{
    public async Task Handle(ProblemCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Problem {ProblemId} created by {EnteredBy}",
            notification.Problem.PrId, notification.Problem.PrEnteredBy);

        await publisher.PublishAsync("problem.exchange", "problem.created", new
        {
            notification.Problem.PrId,
            notification.Problem.PrDescription,
            notification.Problem.PrOwner,
            notification.Problem.PrUnitId,
            notification.Problem.PrSiteId
        }, ct);
    }
}

public class ProblemApprovedEventHandler(
    IMessagePublisher publisher,
    ILogger<ProblemApprovedEventHandler> logger) : INotificationHandler<ProblemApprovedEvent>
{
    public async Task Handle(ProblemApprovedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Problem {ProblemId} approved by {ApprovedBy}",
            notification.Problem.PrId, notification.ApprovedBy);

        await publisher.PublishAsync("problem.exchange", "problem.approved", new
        {
            notification.Problem.PrId,
            notification.ApprovedBy,
            notification.Reason
        }, ct);
    }
}

public class ProblemRejectedEventHandler(
    IMessagePublisher publisher,
    ILogger<ProblemRejectedEventHandler> logger) : INotificationHandler<ProblemRejectedEvent>
{
    public async Task Handle(ProblemRejectedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Problem {ProblemId} rejected by {RejectedBy}",
            notification.Problem.PrId, notification.RejectedBy);

        await publisher.PublishAsync("problem.exchange", "problem.rejected", new
        {
            notification.Problem.PrId,
            notification.RejectedBy,
            notification.Reason
        }, ct);
    }
}

public class SolutionAddedEventHandler(
    IMessagePublisher publisher,
    ILogger<SolutionAddedEventHandler> logger) : INotificationHandler<SolutionAddedEvent>
{
    public async Task Handle(SolutionAddedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Solution {SolutionId} added for Problem {ProblemId}",
            notification.Solution.SolId, notification.Problem.PrId);

        await publisher.PublishAsync("problem.exchange", "solution.added", new
        {
            notification.Solution.SolId,
            notification.Problem.PrId,
            notification.Solution.SolDescription
        }, ct);
    }
}
