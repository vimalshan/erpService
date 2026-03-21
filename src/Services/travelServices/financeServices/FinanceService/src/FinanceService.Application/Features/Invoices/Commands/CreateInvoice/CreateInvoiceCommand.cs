using MediatR;
using FinanceService.Application.DTOs;

namespace FinanceService.Application.Features.Invoices.Commands.CreateInvoice;

public record CreateInvoiceCommand : IRequest<InvoiceDto>
{
    public string? InvoiceNum { get; init; }
    public string? InvoiceTypeLookupCode { get; init; }
    public string? InvoiceDate { get; init; }
    public long? VendorId { get; init; }
    public long? VendorSiteId { get; init; }
    public string? InvoiceAmount { get; init; }
    public string? InvoiceCurrencyCode { get; init; }
    public string? Description { get; init; }
    public decimal? OrgId { get; init; }
    public long? AgencyId { get; init; }
    public List<CreateInvoiceLineCommand> Lines { get; init; } = new();
}

public record CreateInvoiceLineCommand
{
    public string? LineTypeLookupCode { get; init; }
    public decimal? Amount { get; init; }
    public string? Description { get; init; }
    public string? AccountCode { get; init; }
    public string? ProjectCode { get; init; }
    public decimal? SgstAmt { get; init; }
    public decimal? CgstAmt { get; init; }
    public decimal? IgstAmt { get; init; }
}
