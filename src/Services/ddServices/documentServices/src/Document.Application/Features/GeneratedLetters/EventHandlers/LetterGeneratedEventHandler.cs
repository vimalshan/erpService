using MediatR;
using Microsoft.Extensions.Logging;
using Document.Application.Common.Interfaces;
using Document.Domain.Events;

namespace Document.Application.Features.GeneratedLetters.EventHandlers;

public class LetterGeneratedEventHandler : INotificationHandler<LetterGeneratedEvent>
{
    private readonly ILogger<LetterGeneratedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public LetterGeneratedEventHandler(ILogger<LetterGeneratedEventHandler> logger, IMessagePublisher publisher)
        => (_logger, _publisher) = (logger, publisher);

    public async Task Handle(LetterGeneratedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Letter generated for employee pin: {EmployeePin}, type: {LetterType}",
            notification.Letter.EmployeePin, notification.Letter.LetterType);

        await _publisher.PublishAsync(new
        {
            notification.Letter.EmployeePin,
            notification.Letter.LetterType,
            notification.Letter.PrintDate
        }, cancellationToken);
    }
}
