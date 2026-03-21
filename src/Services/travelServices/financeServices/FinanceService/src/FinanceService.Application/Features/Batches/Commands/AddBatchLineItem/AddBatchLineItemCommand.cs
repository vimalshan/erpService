using MediatR;
using FinanceService.Application.DTOs;

namespace FinanceService.Application.Features.Batches.Commands.AddBatchLineItem;

public record AddBatchLineItemCommand : IRequest<bool>
{
    public string UnitCode { get; init; } = string.Empty;
    public decimal BatchNumber { get; init; }
    public long BookingNumber { get; init; }
    public decimal TicketCost { get; init; }
    public decimal GstAmount { get; init; }
}
