using MediatR;
using FinanceService.Application.DTOs;

namespace FinanceService.Application.Features.Payments.Queries.GetPaymentDetails;

public record GetPaymentDetailsQuery : IRequest<List<PaymentDto>>
{
    public string? UnitCode { get; init; }
}
