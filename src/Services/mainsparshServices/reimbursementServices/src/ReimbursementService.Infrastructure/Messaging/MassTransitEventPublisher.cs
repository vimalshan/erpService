using MassTransit;
using ReimbursementService.Domain.Events;
using ReimbursementService.Domain.Interfaces;
using ReimbursementService.Infrastructure.Messaging.Contracts;

namespace ReimbursementService.Infrastructure.Messaging;

public sealed class MassTransitEventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class
    {
        // Map domain events → message contracts before publishing to RabbitMQ
        object? message = @event switch
        {
            ReimbursementSubmittedEvent e => new ReimbursementSubmittedMessage
            {
                ReimId = e.Reimbursement.ReimId,
                RefNo = e.RefNo,
                EmpSysId = e.Reimbursement.EmpSysId,
                Amount = e.Reimbursement.Amount.Amount,
                Currency = e.Reimbursement.Amount.Currency,
                OccurredOn = e.OccurredOn
            },
            ReimbursementApprovedEvent e => new ReimbursementApprovedMessage
            {
                ReimId = e.Reimbursement.ReimId,
                RefNo = e.Reimbursement.ReimRefNo,
                ApprovedBy = e.ApprovedBy,
                ApprovalLevel = e.Reimbursement.ApprovalLevel ?? 0,
                OccurredOn = e.OccurredOn
            },
            ReimbursementPaidEvent e => new ReimbursementPaidMessage
            {
                ReimId = e.Reimbursement.ReimId,
                RefNo = e.Reimbursement.ReimRefNo,
                PaymentDate = e.PaymentDate,
                OccurredOn = e.OccurredOn
            },
            _ => null
        };

        if (message is not null)
            await publishEndpoint.Publish(message, cancellationToken);
    }
}

