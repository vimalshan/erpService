using MediatR;
using FinanceService.Application.DTOs;

namespace FinanceService.Application.Features.Invoices.Commands.UpdateInvoice;

public record UpdateInvoiceCommand : IRequest<InvoiceDto>
{
    public long InvoiceId { get; init; }
    public string? InvoiceNum { get; init; }
    public string? InvoiceTypeLookupCode { get; init; }
    public string? InvoiceAmount { get; init; }
    public string? Description { get; init; }
    public string? Status { get; init; }
}
