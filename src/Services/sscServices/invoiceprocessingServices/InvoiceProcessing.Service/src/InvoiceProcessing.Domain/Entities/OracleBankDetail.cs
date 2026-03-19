using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Entities;

public class OracleBankDetail : BaseEntity
{
    public long BnkId { get; private set; }
    public long DocId { get; private set; }
    public string? Type { get; private set; }
    public string CheckId { get; private set; } = null!;
    public string? Business { get; private set; }
    public string? OrgId { get; private set; }
    public string? VendorSiteId { get; private set; }
    public string? FileName { get; private set; }
    public string? RecordIdentifier { get; private set; }
    public string? TransactionType { get; private set; }
    public string? VendorCode { get; private set; }
    public string? MailTo { get; private set; }
    public string? BeneMailAddress { get; private set; }
    public string? BeneBankAc { get; private set; }
    public string? PayTo { get; private set; }
    public string? CheckDate { get; private set; }
    public string? Amount { get; private set; }
    public string? Hundi { get; private set; }
    public string? Currency { get; private set; }
    public string? PaymentLocation { get; private set; }
    public string? PaymentNumber { get; private set; }
    public string? CheckNumber { get; private set; }
    public string? PaymentDate { get; private set; }
    public string? RecordAnnexure { get; private set; }
    public string? PrintLocation { get; private set; }
    public string? BeneIfsc { get; private set; }
    public string? BeneAccountType { get; private set; }
    public string? BeneBankName { get; private set; }
    public string? BeneBankAc22 { get; private set; }
    public string? BeneBankBranch { get; private set; }
    public string? BeneBankLocation { get; private set; }
    public string? BeneMailId { get; private set; }
    public string? RefNo { get; private set; }
    public string? UtrNo { get; private set; }
    public string? RejectReason1 { get; private set; }
    public string? RejectReason2 { get; private set; }
    public string? StatusLookupCode { get; private set; }

    public DocumentDetail Document { get; private set; } = null!;
}
