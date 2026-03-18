using CSA.Service.Application.DTOs;
using CSA.Service.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CSA.Service.Infrastructure.Messaging;

public class ControlCreatedConsumer : RabbitMqConsumerBase<ControlDto>
{
    private readonly IServiceScopeFactory _scopeFactory;

    protected override string QueueName => "csa.control.created";
    protected override string ExchangeName => "csa.events";
    protected override string RoutingKey => "control.created";

    public ControlCreatedConsumer(
        IConfiguration configuration,
        ILogger<ControlCreatedConsumer> logger,
        IServiceScopeFactory scopeFactory) : base(configuration, logger)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleMessageAsync(ControlDto message, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ControlCreatedConsumer>>();
        logger.LogInformation("Processing ControlCreated event for Control {ControlId}: {Title}", message.ControlId, message.Title);
        await Task.CompletedTask;
    }
}

public class SurveyFeedbackConsumer : RabbitMqConsumerBase<SurveyFeedbackDto>
{
    private readonly IServiceScopeFactory _scopeFactory;

    protected override string QueueName => "csa.survey.feedback";
    protected override string ExchangeName => "csa.events";
    protected override string RoutingKey => "survey.feedback.submitted";

    public SurveyFeedbackConsumer(
        IConfiguration configuration,
        ILogger<SurveyFeedbackConsumer> logger,
        IServiceScopeFactory scopeFactory) : base(configuration, logger)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleMessageAsync(SurveyFeedbackDto message, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SurveyFeedbackConsumer>>();
        logger.LogInformation("Processing SurveyFeedback event for Feedback {FeedbackId}, Status: {Status}", message.FeedbackId, message.Status);
        await Task.CompletedTask;
    }
}
