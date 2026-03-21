using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TravelRequestService.Application.DTOs;

namespace TravelRequestService.Infrastructure.Messaging.Consumers;

public class TravelRequestApprovalConsumer : RabbitMqConsumerBase<TravelRequestApprovalMessage>
{
    private readonly IServiceScopeFactory _scopeFactory;

    protected override string QueueName => "travel-request-approval";

    public TravelRequestApprovalConsumer(
        IConfiguration configuration,
        ILogger<TravelRequestApprovalConsumer> logger,
        IServiceScopeFactory scopeFactory)
        : base(configuration, logger)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleMessageAsync(TravelRequestApprovalMessage message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TravelRequestApprovalConsumer>>();
        logger.LogInformation("Processing approval for travel request {PlanNumber}", message.PlanNumber);

        // Process approval logic here
        await Task.CompletedTask;
    }
}

public class TravelRequestApprovalMessage
{
    public long PlanNumber { get; set; }
    public string CompanyCode { get; set; } = null!;
    public long ApprovedBy { get; set; }
    public decimal ApprovalAmount { get; set; }
    public string? Remarks { get; set; }
}
