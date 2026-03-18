using MediatR;

namespace SettlementService.Application.Commands.AddPayment;

public record AddPaymentCommand : IRequest<Unit>
{
    public long SettlementNumber { get; init; }
    public string PaymentMode { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string? ReferenceNo { get; init; }
}
