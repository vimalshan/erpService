using ExpenseService.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExpenseService.Infrastructure.Messaging.Consumers;

public class ExpenseRecordedConsumer : RabbitMqConsumerBase<TravelExpenseDto>
{
    private readonly IServiceProvider _serviceProvider;

    public ExpenseRecordedConsumer(
        IConfiguration configuration,
        ILogger<ExpenseRecordedConsumer> logger,
        IServiceProvider serviceProvider)
        : base(configuration, logger, "expense.recorded.queue", "expense.exchange", "expense.recorded")
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleMessageAsync(TravelExpenseDto message, CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ExpenseRecordedConsumer>>();
        logger.LogInformation("Expense recorded: Request={RequestNum}, Serial={SerialNum}",
            message.RequestNumber, message.SerialNumber);

        await Task.CompletedTask;
    }
}
