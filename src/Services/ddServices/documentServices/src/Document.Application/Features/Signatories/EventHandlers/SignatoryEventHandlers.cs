using MediatR;
using Microsoft.Extensions.Logging;
using Document.Domain.Events;

namespace Document.Application.Features.Signatories.EventHandlers;

public class SignatoryCreatedEventHandler : INotificationHandler<SignatoryCreatedEvent>
{
    private readonly ILogger<SignatoryCreatedEventHandler> _logger;
    public SignatoryCreatedEventHandler(ILogger<SignatoryCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(SignatoryCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Signatory created: {SignatoryNumber} - {Name}",
            notification.Signatory.SignatoryNumber, notification.Signatory.Name);
        return Task.CompletedTask;
    }
}

public class SignatoryUpdatedEventHandler : INotificationHandler<SignatoryUpdatedEvent>
{
    private readonly ILogger<SignatoryUpdatedEventHandler> _logger;
    public SignatoryUpdatedEventHandler(ILogger<SignatoryUpdatedEventHandler> logger) => _logger = logger;

    public Task Handle(SignatoryUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Signatory updated: {SignatoryNumber}", notification.Signatory.SignatoryNumber);
        return Task.CompletedTask;
    }
}
