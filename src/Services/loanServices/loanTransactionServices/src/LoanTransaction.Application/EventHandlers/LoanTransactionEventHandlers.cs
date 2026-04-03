using MediatR;
using Microsoft.Extensions.Logging;
using LoanTransaction.Domain.Events;
using LoanTransaction.Domain.IntegrationEvents;
using LoanTransaction.Domain.Interfaces;

namespace LoanTransaction.Application.EventHandlers;

public class LoanDisbursedEventHandler : INotificationHandler<LoanDisbursedEvent>
{
    private readonly IMessageBus _bus;
    private readonly ILogger<LoanDisbursedEventHandler> _logger;

    public LoanDisbursedEventHandler(IMessageBus bus, ILogger<LoanDisbursedEventHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(LoanDisbursedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Loan disbursed – LoanNo: {LoanNo}, Employee: {EmpId}, Amount: {Amount}",
            notification.LoanNo, notification.EmployeeId, notification.PrincipalAmount);

        await _bus.PublishAsync(new LoanDisbursedIntegrationEvent
        {
            LoanNo = notification.LoanNo,
            ApplicationId = notification.ApplicationId,
            EmployeeId = notification.EmployeeId,
            PrincipalAmount = notification.PrincipalAmount,
            DisbursedAt = notification.DisbursedAt,
            OccurredOn = notification.OccurredAt
        }, "loan.transaction.disbursed", ct);
    }
}

public class EmiPaymentRecordedEventHandler : INotificationHandler<EmiPaymentRecordedEvent>
{
    private readonly IMessageBus _bus;
    private readonly ILogger<EmiPaymentRecordedEventHandler> _logger;

    public EmiPaymentRecordedEventHandler(IMessageBus bus, ILogger<EmiPaymentRecordedEventHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(EmiPaymentRecordedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("EMI payment recorded – LoanNo: {LoanNo}, Installment: {InstNo}, Principal: {Prn}, Interest: {Int}, Outstanding: {Out}",
            notification.LoanNo, notification.InstallmentNo, notification.PrincipalPaid,
            notification.InterestPaid, notification.PrincipalOutstanding);

        await _bus.PublishAsync(new EmiPaidIntegrationEvent
        {
            LoanNo = notification.LoanNo,
            InstallmentNo = notification.InstallmentNo,
            PrincipalPaid = notification.PrincipalPaid,
            InterestPaid = notification.InterestPaid,
            PrincipalOutstanding = notification.PrincipalOutstanding,
            OccurredOn = notification.OccurredAt
        }, "loan.transaction.emi.paid", ct);
    }
}

public class LoanClosedEventHandler : INotificationHandler<LoanClosedEvent>
{
    private readonly IMessageBus _bus;
    private readonly ILogger<LoanClosedEventHandler> _logger;

    public LoanClosedEventHandler(IMessageBus bus, ILogger<LoanClosedEventHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(LoanClosedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Loan closed – LoanNo: {LoanNo}, Employee: {EmpId}, ClosureType: {Type}, ClosedAt: {At}",
            notification.LoanNo, notification.EmployeeId, notification.ClosureType, notification.ClosedAt);

        await _bus.PublishAsync(new LoanClosedIntegrationEvent
        {
            LoanNo = notification.LoanNo,
            EmployeeId = notification.EmployeeId,
            ClosureType = notification.ClosureType,
            ClosedAt = notification.ClosedAt,
            OccurredOn = notification.OccurredAt
        }, "loan.transaction.closed", ct);
    }
}
