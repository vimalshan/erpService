using MassTransit;
using Microsoft.Extensions.Logging;

namespace RecruitmentService.Infrastructure.Messaging.Consumers;

public record ApplicationSubmittedMessage(decimal AppId, decimal VacancyId, DateTime OccurredOn)
{
    public ApplicationSubmittedMessage() : this(default, default, default) { }
}

public class ApplicationSubmittedConsumer : IConsumer<ApplicationSubmittedMessage>
{
    private readonly ILogger<ApplicationSubmittedConsumer> _logger;

    public ApplicationSubmittedConsumer(ILogger<ApplicationSubmittedConsumer> logger) => _logger = logger;

    public async Task Consume(ConsumeContext<ApplicationSubmittedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation(
            "[RabbitMQ] Application {AppId} submitted for Vacancy {VacancyId} at {OccurredOn}",
            msg.AppId, msg.VacancyId, msg.OccurredOn);

        // Place downstream processing here: notify HR, update analytics, etc.
        await Task.CompletedTask;
    }
}

public record VacancyCreatedMessage(decimal VacancyId, string VacancyName, string Unit, DateTime OccurredOn)
{
    public VacancyCreatedMessage() : this(default, default!, default!, default) { }
}

public class VacancyCreatedConsumer : IConsumer<VacancyCreatedMessage>
{
    private readonly ILogger<VacancyCreatedConsumer> _logger;

    public VacancyCreatedConsumer(ILogger<VacancyCreatedConsumer> logger) => _logger = logger;

    public async Task Consume(ConsumeContext<VacancyCreatedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation(
            "[RabbitMQ] Vacancy created: {VacancyId} - {VacancyName} for unit {Unit}",
            msg.VacancyId, msg.VacancyName, msg.Unit);

        await Task.CompletedTask;
    }
}

public record ApplicationStatusChangedMessage(decimal AppId, string PreviousStatus, string CurrentStatus, DateTime OccurredOn)
{
    public ApplicationStatusChangedMessage() : this(default, default!, default!, default) { }
}

public class ApplicationStatusChangedConsumer : IConsumer<ApplicationStatusChangedMessage>
{
    private readonly ILogger<ApplicationStatusChangedConsumer> _logger;

    public ApplicationStatusChangedConsumer(ILogger<ApplicationStatusChangedConsumer> logger) => _logger = logger;

    public async Task Consume(ConsumeContext<ApplicationStatusChangedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation(
            "[RabbitMQ] Application {AppId} status changed from {Prev} to {Curr}",
            msg.AppId, msg.PreviousStatus, msg.CurrentStatus);

        await Task.CompletedTask;
    }
}
