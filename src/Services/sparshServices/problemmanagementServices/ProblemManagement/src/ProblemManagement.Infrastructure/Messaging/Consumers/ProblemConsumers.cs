using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProblemManagement.Application.Commands;
using ProblemManagement.Infrastructure.Messaging;

namespace ProblemManagement.Infrastructure.Messaging.Consumers;

public record ProblemCreatedMessage(long ProblemId, string Description, long Owner, long UnitId, long SiteId);

public class ProblemCreatedConsumer : RabbitMqConsumerBase<ProblemCreatedMessage>
{
    public ProblemCreatedConsumer(IServiceProvider serviceProvider, ILogger<ProblemCreatedConsumer> logger)
        : base(serviceProvider, logger, "problem.created.queue", "problem.exchange", "problem.created")
    {
    }

    protected override async Task HandleMessageAsync(ProblemCreatedMessage message, IServiceProvider serviceProvider, CancellationToken ct)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var logger = serviceProvider.GetRequiredService<ILogger<ProblemCreatedConsumer>>();
        logger.LogInformation("Processing ProblemCreated message for ProblemId: {ProblemId}", message.ProblemId);
        // Handle the message - e.g., send notifications, update dashboards, etc.
        await Task.CompletedTask;
    }
}

public record SolutionApprovedMessage(long SolutionId, long ProblemId, long ApprovedBy);

public class SolutionApprovedConsumer : RabbitMqConsumerBase<SolutionApprovedMessage>
{
    public SolutionApprovedConsumer(IServiceProvider serviceProvider, ILogger<SolutionApprovedConsumer> logger)
        : base(serviceProvider, logger, "solution.approved.queue", "problem.exchange", "solution.approved")
    {
    }

    protected override async Task HandleMessageAsync(SolutionApprovedMessage message, IServiceProvider serviceProvider, CancellationToken ct)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<SolutionApprovedConsumer>>();
        logger.LogInformation("Processing SolutionApproved message for SolutionId: {SolutionId}", message.SolutionId);
        await Task.CompletedTask;
    }
}
