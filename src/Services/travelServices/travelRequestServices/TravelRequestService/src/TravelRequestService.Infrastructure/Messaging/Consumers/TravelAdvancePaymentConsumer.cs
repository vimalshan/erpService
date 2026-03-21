using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TravelRequestService.Infrastructure.Messaging.Consumers;

public class TravelAdvancePaymentConsumer : RabbitMqConsumerBase<TravelAdvancePaymentMessage>
{
    private readonly IServiceScopeFactory _scopeFactory;

    protected override string QueueName => "travel-advance-payment";

    public TravelAdvancePaymentConsumer(
        IConfiguration configuration,
        ILogger<TravelAdvancePaymentConsumer> logger,
        IServiceScopeFactory scopeFactory)
        : base(configuration, logger)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleMessageAsync(TravelAdvancePaymentMessage message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TravelAdvancePaymentConsumer>>();
        logger.LogInformation("Processing advance payment for request {RequestNumber}", message.RequestNumber);

        // Process advance payment logic here
        await Task.CompletedTask;
    }
}

public class TravelAdvancePaymentMessage
{
    public long RequestNumber { get; set; }
    public long AdvanceNumber { get; set; }
    public decimal PaidAmount { get; set; }
    public string PayType { get; set; } = null!;
}
