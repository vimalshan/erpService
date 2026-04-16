using FinanceService.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FinanceService.Infrastructure.Messaging;

public class InvoiceCreatedConsumer : IConsumer<InvoiceCreatedEvent>
{
    private readonly ILogger<InvoiceCreatedConsumer> _logger;
    public InvoiceCreatedConsumer(ILogger<InvoiceCreatedConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<InvoiceCreatedEvent> context)
    {
        _logger.LogInformation("Invoice created: {InvoiceId} - {InvoiceNumber}, Amount: {Amount}", context.Message.InvoiceId, context.Message.InvoiceNumber, context.Message.TotalAmount);
        return Task.CompletedTask;
    }
}

public class InvoicePaidConsumer : IConsumer<InvoicePaidEvent>
{
    private readonly ILogger<InvoicePaidConsumer> _logger;
    public InvoicePaidConsumer(ILogger<InvoicePaidConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<InvoicePaidEvent> context)
    {
        _logger.LogInformation("Invoice paid: {InvoiceId} - {InvoiceNumber} on {PaidDate}", context.Message.InvoiceId, context.Message.InvoiceNumber, context.Message.PaidDate);
        return Task.CompletedTask;
    }
}

public class InvoiceOverdueConsumer : IConsumer<InvoiceOverdueEvent>
{
    private readonly ILogger<InvoiceOverdueConsumer> _logger;
    public InvoiceOverdueConsumer(ILogger<InvoiceOverdueConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<InvoiceOverdueEvent> context)
    {
        _logger.LogInformation("Invoice overdue: {InvoiceId} - {InvoiceNumber}, Due: {DueDate}", context.Message.InvoiceId, context.Message.InvoiceNumber, context.Message.DueDate);
        return Task.CompletedTask;
    }
}
