using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using CashManagement.Application.Commands.CashTransaction;
using CashManagement.Application.Commands.ChequeRegister;

namespace CashManagement.Infrastructure.Messaging.RabbitMQ.Consumers;

public class CashReceiptMessageConsumer : RabbitMqConsumerBase<CashReceiptMessage>
{
    private readonly IMediator _mediator;

    public CashReceiptMessageConsumer(IConnection connection, IChannel channel,
        IMediator mediator, ILogger<CashReceiptMessageConsumer> logger)
        : base(connection, channel, logger)
    {
        _mediator = mediator;
    }

    protected override async Task ProcessMessageAsync(CashReceiptMessage message, CancellationToken ct)
    {
        Logger.LogInformation("Processing CashReceipt message for unit {UnitId}", message.CashUnitId);
        await _mediator.Send(new RecordCashReceiptCommand(
            message.CashUnitId, message.Amount, message.Source,
            message.RefNo, message.Remarks, message.CreatedBy), ct);
    }
}

public class ChequeBounceMessageConsumer : RabbitMqConsumerBase<ChequeBounceMessage>
{
    private readonly IMediator _mediator;

    public ChequeBounceMessageConsumer(IConnection connection, IChannel channel,
        IMediator mediator, ILogger<ChequeBounceMessageConsumer> logger)
        : base(connection, channel, logger)
    {
        _mediator = mediator;
    }

    protected override async Task ProcessMessageAsync(ChequeBounceMessage message, CancellationToken ct)
    {
        Logger.LogInformation("Processing ChequeBounce message for cheque {ChequeId}", message.ChequeId);
        await _mediator.Send(new MarkChequeBouncedCommand(
            message.ChequeId, message.BounceReason, message.ProcessedBy), ct);
    }
}

// Message contracts
public record CashReceiptMessage(long CashUnitId, decimal Amount, string? Source, string? RefNo, string? Remarks, long CreatedBy);
public record ChequeBounceMessage(long ChequeId, string BounceReason, long ProcessedBy);
