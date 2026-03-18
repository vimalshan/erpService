using Microsoft.Extensions.Logging;

namespace LoanService.Infrastructure.Messaging.Consumers;

public record LoanPaymentMessage(long LoanNo, long RepaymentId, decimal Amount, DateTime PaidDate);

public class LoanPaymentConsumer : RabbitMqConsumer<LoanPaymentMessage>
{
    private readonly ILogger<LoanPaymentConsumer> _logger;

    public LoanPaymentConsumer(string hostName, string userName, string password, ILogger<LoanPaymentConsumer> logger)
        : base(hostName, userName, password, "loan-payment-queue", "loan-exchange", "loan.payment.#", logger)
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(LoanPaymentMessage message, CancellationToken ct)
    {
        _logger.LogInformation("Received payment for Loan {LoanNo}, Repayment {RepaymentId}, Amount {Amount}",
            message.LoanNo, message.RepaymentId, message.Amount);
        return Task.CompletedTask;
    }
}

public record LoanApprovalMessage(long LoanNo, DateTime ApprovalDate);

public class LoanApprovalConsumer : RabbitMqConsumer<LoanApprovalMessage>
{
    private readonly ILogger<LoanApprovalConsumer> _logger;

    public LoanApprovalConsumer(string hostName, string userName, string password, ILogger<LoanApprovalConsumer> logger)
        : base(hostName, userName, password, "loan-approval-queue", "loan-exchange", "loan.approved.#", logger)
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(LoanApprovalMessage message, CancellationToken ct)
    {
        _logger.LogInformation("Loan {LoanNo} approved on {ApprovalDate}", message.LoanNo, message.ApprovalDate);
        return Task.CompletedTask;
    }
}
