using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentSharePoint : BaseEntity
{
    public long? SharePointId { get; private set; }
    public string? Unit { get; private set; }
    public string? Status { get; private set; }
    public string? Category { get; private set; }
    public string? SubCategory { get; private set; }
    public string? Business { get; private set; }
    public string? VendorNameSite { get; private set; }
    public string? VendorName { get; private set; }
    public string? VendorSite { get; private set; }
    public string? PoNo { get; private set; }
    public string? MrcNo { get; private set; }
    public string? R12Voucher { get; private set; }
    public string? Currency { get; private set; }
    public string? Amount { get; private set; }
    public string? DocKey { get; private set; }
    public string? InvNo { get; private set; }
    public string? InvDate { get; private set; }
    public string? PayTo { get; private set; }
    public string? VendorCode { get; private set; }
    public string? R12BuCode { get; private set; }
}
