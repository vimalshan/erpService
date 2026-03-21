namespace FinanceService.Application.DTOs;

public class InvoiceDto
{
    public long InvoiceId { get; set; }
    public string? InvoiceNum { get; set; }
    public string? InvoiceTypeLookupCode { get; set; }
    public string? InvoiceDate { get; set; }
    public long? VendorId { get; set; }
    public long? VendorSiteId { get; set; }
    public string? InvoiceAmount { get; set; }
    public string? InvoiceCurrencyCode { get; set; }
    public string? ExchangeRate { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public long? AgencyId { get; set; }
    public List<InvoiceLineDto> Lines { get; set; } = new();
}

public class InvoiceLineDto
{
    public long InvoiceId { get; set; }
    public long? InvoiceLineId { get; set; }
    public long LineNumber { get; set; }
    public string? LineTypeLookupCode { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public string? AccountCode { get; set; }
    public string? ProjectCode { get; set; }
    public decimal? SgstAmt { get; set; }
    public decimal? CgstAmt { get; set; }
    public decimal? IgstAmt { get; set; }
}
