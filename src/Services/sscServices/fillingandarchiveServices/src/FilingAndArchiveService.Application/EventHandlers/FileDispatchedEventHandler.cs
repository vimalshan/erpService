using FilingAndArchiveService.Application.Common.Interfaces;
using FilingAndArchiveService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FilingAndArchiveService.Application.EventHandlers;

public class FileDispatchedEventHandler : INotificationHandler<FileDispatchedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<FileDispatchedEventHandler> _logger;

    public FileDispatchedEventHandler(IMessagePublisher publisher, ILogger<FileDispatchedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(FileDispatchedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Filing Archive: File {FileId} ({FileNo}) dispatched via {Courier} with POD {PodNo}",
            notification.FileId,
            notification.FileNo,
            notification.CourierName,
            notification.PodNo);

        await _publisher.PublishAsync(
            "filing-archive",
            "file.dispatched",
            new
            {
                notification.FileId,
                notification.OrgId,
                notification.FileNo,
                notification.CourierName,
                notification.PodNo,
                notification.OccurredOn
            },
            cancellationToken);
    }
}
