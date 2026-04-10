using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SparshTransactional.Infrastructure.Messaging.Consumers;

public record ApplicationApprovedMessage(long ApplicationId, long StudentId, long ScholarshipId, decimal ApprovedAmount);

public class ApplicationApprovedConsumer : RabbitMqConsumerBase<ApplicationApprovedMessage>
{
    public ApplicationApprovedConsumer(IServiceProvider serviceProvider, ILogger<ApplicationApprovedConsumer> logger)
        : base(serviceProvider, logger, "scholarship.application.approved.queue", "scholarship.exchange", "application.approved")
    {
    }

    protected override async Task HandleMessageAsync(ApplicationApprovedMessage message, IServiceProvider serviceProvider, CancellationToken ct)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<ApplicationApprovedConsumer>>();
        logger.LogInformation("Processing ApplicationApproved message for ApplicationId: {ApplicationId}, Amount: {Amount}",
            message.ApplicationId, message.ApprovedAmount);
        await Task.CompletedTask;
    }
}

public record DisbursementCompletedMessage(long DisbursementId, long StudentId, decimal Amount, string PaymentReference);

public class DisbursementCompletedConsumer : RabbitMqConsumerBase<DisbursementCompletedMessage>
{
    public DisbursementCompletedConsumer(IServiceProvider serviceProvider, ILogger<DisbursementCompletedConsumer> logger)
        : base(serviceProvider, logger, "scholarship.disbursement.completed.queue", "scholarship.exchange", "disbursement.completed")
    {
    }

    protected override async Task HandleMessageAsync(DisbursementCompletedMessage message, IServiceProvider serviceProvider, CancellationToken ct)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<DisbursementCompletedConsumer>>();
        logger.LogInformation("Processing DisbursementCompleted message for DisbursementId: {DisbursementId}, Ref: {Ref}",
            message.DisbursementId, message.PaymentReference);
        await Task.CompletedTask;
    }
}
