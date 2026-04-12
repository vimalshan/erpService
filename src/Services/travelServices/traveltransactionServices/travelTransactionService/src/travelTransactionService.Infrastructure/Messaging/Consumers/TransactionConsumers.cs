using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace travelTransactionService.Infrastructure.Messaging.Consumers;

public class VendorUpdateConsumer : RabbitMqConsumerBase<VendorUpdateMessage>
{
    private readonly IServiceScopeFactory _scopeFactory;

    protected override string QueueName => "vendor-update";

    public VendorUpdateConsumer(
        IConfiguration configuration,
        ILogger<VendorUpdateConsumer> logger,
        IServiceScopeFactory scopeFactory)
        : base(configuration, logger)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleMessageAsync(VendorUpdateMessage message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<VendorUpdateConsumer>>();
        logger.LogInformation("Processing vendor update for VendorId {VendorId}", message.VendorId);

        // Process vendor update logic here
        await Task.CompletedTask;
    }
}

public class VendorUpdateMessage
{
    public long VendorId { get; set; }
    public string Name { get; set; } = null!;
    public string CategoryType { get; set; } = null!;
}

public class TaxCalculationConsumer : RabbitMqConsumerBase<TaxCalculationMessage>
{
    private readonly IServiceScopeFactory _scopeFactory;

    protected override string QueueName => "tax-calculation";

    public TaxCalculationConsumer(
        IConfiguration configuration,
        ILogger<TaxCalculationConsumer> logger,
        IServiceScopeFactory scopeFactory)
        : base(configuration, logger)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleMessageAsync(TaxCalculationMessage message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TaxCalculationConsumer>>();
        logger.LogInformation("Processing tax calculation for TransactionNum {TransactionNum}", message.TransactionNum);

        // Process tax calculation logic here
        await Task.CompletedTask;
    }
}

public class TaxCalculationMessage
{
    public string TransactionNum { get; set; } = null!;
    public decimal TransactionLineNum { get; set; }
    public decimal? SgstAmount { get; set; }
    public decimal? CgstAmount { get; set; }
    public decimal? IgstAmount { get; set; }
}
