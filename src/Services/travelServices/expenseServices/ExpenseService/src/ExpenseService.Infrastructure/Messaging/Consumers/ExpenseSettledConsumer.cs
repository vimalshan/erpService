using ExpenseService.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExpenseService.Infrastructure.Messaging.Consumers;

public class ExpenseSettledConsumer : RabbitMqConsumerBase<SettlementResultDto>
{
    private readonly IServiceProvider _serviceProvider;

    public ExpenseSettledConsumer(
        IConfiguration configuration,
        ILogger<ExpenseSettledConsumer> logger,
        IServiceProvider serviceProvider)
        : base(configuration, logger, "expense.settled.queue", "expense.exchange", "expense.settled")
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleMessageAsync(SettlementResultDto message, CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // Process settlement notification (e.g., send email, update accounting system)
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ExpenseSettledConsumer>>();
        logger.LogInformation("Processing settlement: Amount={Amount}, Refund={Refund}",
            message.SettlementAmount, message.RefundAmount);

        await Task.CompletedTask;
    }
}
