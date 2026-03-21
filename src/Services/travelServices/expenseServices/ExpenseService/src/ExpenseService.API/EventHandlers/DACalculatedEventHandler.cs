using ExpenseService.Application.Interfaces;
using ExpenseService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseService.API.EventHandlers;

public class DACalculatedEventHandler : INotificationHandler<DACalculatedEvent>
{
    private readonly ILogger<DACalculatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public DACalculatedEventHandler(ILogger<DACalculatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(DACalculatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: DA Calculated - Request: {RequestId}, Total DA: {TotalDA}",
            notification.RequestId, notification.TotalDAAmount);

        await _publisher.PublishAsync("expense.exchange", "expense.da.calculated", notification, cancellationToken);
    }
}
