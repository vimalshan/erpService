using MediatR;
using FinanceService.Application.DTOs;

namespace FinanceService.Application.Features.Batches.Commands.CreateBatch;

public record CreateBatchCommand : IRequest<BatchDto>
{
    public string UnitCode { get; init; } = string.Empty;
    public long AgencyCode { get; init; }
    public decimal? TotalAmount { get; init; }
    public string? InvoiceNum { get; init; }
    public string? AdminRemarks { get; init; }
}
