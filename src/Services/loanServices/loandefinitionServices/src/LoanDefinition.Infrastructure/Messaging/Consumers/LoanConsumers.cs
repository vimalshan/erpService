using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LoanDefinition.Infrastructure.Messaging.Consumers;

public record LoanApprovalMessage(long LoanId, long EmployeeId, string Status);

public class LoanApprovalConsumer(IOptions<RabbitMqSettings> settings, ILogger<LoanApprovalConsumer> logger)
    : RabbitMqConsumerBase<LoanApprovalMessage>(settings, logger, "loan-definition-approval")
{
    protected override Task HandleMessageAsync(LoanApprovalMessage message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing loan approval: LoanId={LoanId}, Employee={EmployeeId}, Status={Status}",
            message.LoanId, message.EmployeeId, message.Status);
        return Task.CompletedTask;
    }
}

public record LoanRateUpdateMessage(long LoanId, int NewRate, DateTime EffectiveDate);

public class LoanRateUpdateConsumer(IOptions<RabbitMqSettings> settings, ILogger<LoanRateUpdateConsumer> logger)
    : RabbitMqConsumerBase<LoanRateUpdateMessage>(settings, logger, "loan-definition-rate-update")
{
    protected override Task HandleMessageAsync(LoanRateUpdateMessage message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing rate update: LoanId={LoanId}, NewRate={NewRate}%, EffDate={EffDate}",
            message.LoanId, message.NewRate, message.EffectiveDate);
        return Task.CompletedTask;
    }
}
