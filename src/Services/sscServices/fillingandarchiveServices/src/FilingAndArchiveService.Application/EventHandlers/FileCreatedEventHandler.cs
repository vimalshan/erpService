using FilingAndArchiveService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FilingAndArchiveService.Application.EventHandlers;

public class FileCreatedEventHandler : INotificationHandler<FileCreatedEvent>
{
    private readonly ILogger<FileCreatedEventHandler> _logger;

    public FileCreatedEventHandler(ILogger<FileCreatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(FileCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Filing Archive: File {FileId} created for org {OrgId} with FileNo {FileNo} at {OccurredOn}",
            notification.FileId,
            notification.OrgId,
            notification.FileNo,
            notification.OccurredOn);

        return Task.CompletedTask;
    }
}
