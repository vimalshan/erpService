using ContributionService.Application.Commands.ContributionBatch;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContributionService.Infrastructure.Messaging;

public record ContributionBatchMessage(string MonthYear, long ProcessedByUserId);

public class ContributionBatchConsumer(
    IConfiguration configuration,
    ILogger<ContributionBatchConsumer> logger,
    IServiceScopeFactory scopeFactory)
    : RabbitMqConsumerBase<ContributionBatchMessage>(configuration, logger)
{
    protected override string QueueName => "contribution.batch.process";
    protected override string ExchangeName => "contribution.exchange";
    protected override string RoutingKey => "contribution.batch.#";

    protected override async Task HandleMessageAsync(ContributionBatchMessage message, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(new ProcessMonthlyContributionCommand(message.MonthYear, message.ProcessedByUserId), ct);
        logger.LogInformation("Processed monthly contribution for {MonthYear}", message.MonthYear);
    }
}

public record ContributionPostMessage(long BatchNo, long PostedByUserId);

public class ContributionPostConsumer(
    IConfiguration configuration,
    ILogger<ContributionPostConsumer> logger,
    IServiceScopeFactory scopeFactory)
    : RabbitMqConsumerBase<ContributionPostMessage>(configuration, logger)
{
    protected override string QueueName => "contribution.batch.post";
    protected override string ExchangeName => "contribution.exchange";
    protected override string RoutingKey => "contribution.post.#";

    protected override async Task HandleMessageAsync(ContributionPostMessage message, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(new PostContributionBatchCommand(message.BatchNo, message.PostedByUserId), ct);
        logger.LogInformation("Posted contribution batch {BatchNo}", message.BatchNo);
    }
}
