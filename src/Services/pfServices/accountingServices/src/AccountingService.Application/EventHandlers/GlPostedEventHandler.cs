using AccountingService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccountingService.Application.EventHandlers;

public class GlPostedEventHandler : INotificationHandler<GlPostedEvent>
{
    private readonly ILogger<GlPostedEventHandler> _logger;

    public GlPostedEventHandler(ILogger<GlPostedEventHandler> logger)
        => _logger = logger;

    public Task Handle(GlPostedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: GL entry posted. AccountCode={AccountCode}, Debit={Debit}, Credit={Credit}, ReferenceId={ReferenceId}",
            notification.GlPosting.AccountCode,
            notification.GlPosting.DebitAmount,
            notification.GlPosting.CreditAmount,
            notification.GlPosting.ReferenceId);

        return Task.CompletedTask;
    }
}
