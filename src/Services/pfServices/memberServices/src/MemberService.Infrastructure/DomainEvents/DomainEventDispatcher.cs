using MemberService.Domain.Common;
using MemberService.Domain.Events;
using MemberService.Domain.Interfaces;
using MemberService.Infrastructure.Messaging;
using MemberService.Infrastructure.Messaging.Events;
using Microsoft.Extensions.Logging;

namespace MemberService.Infrastructure.DomainEvents;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IRabbitMqPublisher _publisher;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IRabbitMqPublisher publisher, ILogger<DomainEventDispatcher> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct = default)
    {
        foreach (var evt in domainEvents)
        {
            _logger.LogInformation("Dispatching domain event: {EventType}", evt.EventType);
            try
            {
                switch (evt)
                {
                    case MemberCreatedEvent e:
                        await _publisher.PublishAsync(
                            new MemberCreatedMessage(e.MemberNo, e.MemberName, e.TrustCode, e.OccurredOn),
                            "member.exchange", "member.created", ct);
                        break;
                    case MemberClosedEvent e:
                        await _publisher.PublishAsync(
                            new MemberClosedMessage(e.MemberNo, e.LeaveReason, e.LeaveDate, e.OccurredOn),
                            "member.exchange", "member.closed", ct);
                        break;
                    case NomineeAddedEvent e:
                        await _publisher.PublishAsync(
                            new NomineeAddedMessage(e.MemberNo, e.SerialNo, e.NomineeName, e.Percentage, e.FundType, e.OccurredOn),
                            "member.exchange", "member.nominee.added", ct);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to dispatch domain event {EventType}", evt.EventType);
                // Non-fatal: don't block the transaction
            }
        }
    }
}
