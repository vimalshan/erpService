using MediatR;
using FinanceService.Application.DTOs;

namespace FinanceService.Application.Features.Payments.Commands.ProcessPayment;

public record ProcessPaymentCommand : IRequest<PaymentDto>
{
    public decimal BatchNumber { get; init; }
    public decimal PaymentAmount { get; init; }
    public string PaymentMode { get; init; } = string.Empty;
    public string? ChequeNum { get; init; }
    public long? ProcessedBy { get; init; }
}
