using EnergyService.Application.Common.Interfaces;
using EnergyService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnergyService.Application.EventHandlers;

public class ReadingRecordedEventHandler : INotificationHandler<ReadingRecordedEvent>
{
    private readonly IRabbitMqPublisher _publisher;
    private readonly ILogger<ReadingRecordedEventHandler> _logger;

    public ReadingRecordedEventHandler(IRabbitMqPublisher publisher, ILogger<ReadingRecordedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(ReadingRecordedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: Reading recorded for Process {ProcessId}, Usage: {Usage}",
            notification.ProcessId, notification.ActualUsage);

        await _publisher.PublishAsync("energy-exchange", "reading.recorded", notification, ct);
    }
}
