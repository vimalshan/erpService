using ContractService.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ContractService.Infrastructure.Messaging;

public class ContractCreatedConsumer : IConsumer<ContractCreatedEvent>
{
    private readonly ILogger<ContractCreatedConsumer> _logger;
    public ContractCreatedConsumer(ILogger<ContractCreatedConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<ContractCreatedEvent> context)
    {
        _logger.LogInformation("Contract created: {ContractId} - {ContractNumber}", context.Message.ContractId, context.Message.ContractNumber);
        return Task.CompletedTask;
    }
}

public class ContractStatusChangedConsumer : IConsumer<ContractStatusChangedEvent>
{
    private readonly ILogger<ContractStatusChangedConsumer> _logger;
    public ContractStatusChangedConsumer(ILogger<ContractStatusChangedConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<ContractStatusChangedEvent> context)
    {
        _logger.LogInformation("Contract {ContractId} status changed: {Old} -> {New}", context.Message.ContractId, context.Message.OldStatus, context.Message.NewStatus);
        return Task.CompletedTask;
    }
}

public class ContractRenewedConsumer : IConsumer<ContractRenewedEvent>
{
    private readonly ILogger<ContractRenewedConsumer> _logger;
    public ContractRenewedConsumer(ILogger<ContractRenewedConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<ContractRenewedEvent> context)
    {
        _logger.LogInformation("Contract renewed: {ContractId} - {ContractNumber}", context.Message.ContractId, context.Message.ContractNumber);
        return Task.CompletedTask;
    }
}
