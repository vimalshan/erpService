using InvoiceProcessing.Domain.Common;
using InvoiceProcessing.Domain.Events;

namespace InvoiceProcessing.Domain.Entities;

public class DocumentDetail : AggregateRoot
{
    public string OrgId { get; private set; } = null!;
    public int LocationId { get; private set; }
    public string? DocumentNo { get; private set; }
    public string DocumentType { get; private set; } = null!;
    public long MainCategory { get; private set; }
    public long SubCategory { get; private set; }
    public string PoNumber { get; private set; } = null!;
    public long VendorSiteId { get; private set; }
    public long VendorId { get; private set; }
    public int DueDays { get; private set; }
    public long PoId { get; private set; }
    public string? MrcRemarks { get; private set; }
    public string VatFlag { get; private set; } = "N";
    public string InvoiceNo { get; private set; } = null!;
    public long InvoiceAmount { get; private set; }
    public int Currency { get; private set; }
    public DateTime InvoiceDate { get; private set; }
    public DateTime InvoiceReceiptDate { get; private set; }
    public long Pages { get; private set; }
    public string? Remarks { get; private set; }
    public DateTime PaymentDueDate { get; private set; }
    public int PayBy { get; private set; }
    public long? Signatory1 { get; private set; }
    public long? Signatory2 { get; private set; }
    public long? Approver { get; private set; }
    public long Owner { get; private set; }
    public string DocumentStatus { get; private set; } = null!;
    public string? InvoiceStatus { get; private set; }
    public long UserId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime SubmittedOn { get; private set; }
    public long ReceivedBy { get; private set; }
    public DateTime ReceivedOn { get; private set; }
    public string CancelFlag { get; private set; } = "N";
    public long CancelUser { get; private set; }
    public DateTime CancelDate { get; private set; }
    public long CurrentAllocationId { get; private set; }
    public long OracleVoucherNo { get; private set; }
    public long PaymentTypeNo { get; private set; }
    public string AccountCode { get; private set; } = null!;
    public string? SscInvoicePdf { get; private set; }
    public string? DocumentKey { get; private set; }
    public string? UserInvoicePdf { get; private set; }
    public string? FilePath { get; private set; }
    public DateTime? InvoiceProcessedDate { get; private set; }
    public long? InvoiceProcessedAllocationId { get; private set; }
    public DateTime? InvoiceValidationDate { get; private set; }
    public long? InvoiceValidationAllocationId { get; private set; }
    public string? HoldStatus { get; private set; }
    public long? Deduction { get; private set; }
    public string? ThirdPartyFlag { get; private set; }
    public string? ThirdPartyVendor { get; private set; }
    public string? DeductionRemarks { get; private set; }
    public long? FileId { get; private set; }
    public string? CancelRemarks { get; private set; }
    public string? HoldPaymentFlag { get; private set; }
    public string? HoldPaymentRemarks { get; private set; }
    public string? HoldReleaseRemarks { get; private set; }
    public string? ScanFlag { get; private set; }
    public long? ApprovedBy { get; private set; }

    // Navigation properties
    public ICollection<OracleInvoiceDetail> OracleInvoiceDetails { get; private set; } = [];
    public ICollection<OraclePaymentDetail> OraclePaymentDetails { get; private set; } = [];
    public ICollection<OracleBankDetail> OracleBankDetails { get; private set; } = [];
    public ICollection<DocumentPoList> PoList { get; private set; } = [];
    public ICollection<DocumentApprovalDetail> ApprovalDetails { get; private set; } = [];
    public ICollection<DocumentMrcList> MrcList { get; private set; } = [];
    public ICollection<DocumentCostCenter> CostCenters { get; private set; } = [];
    public ICollection<DocumentAttachment> Attachments { get; private set; } = [];
    public ICollection<DocumentApAllocation> ApAllocations { get; private set; } = [];
    public ICollection<DocumentCorrespondence> Correspondences { get; private set; } = [];
    public ICollection<DocumentRescanDetail> RescanDetails { get; private set; } = [];
    public ICollection<DocumentRevokeDetail> RevokeDetails { get; private set; } = [];
    public ICollection<OracleDueDetail> OracleDueDetails { get; private set; } = [];
    public ICollection<DocumentSscFile> SscFiles { get; private set; } = [];

    private DocumentDetail() { }

    public static DocumentDetail Create(
        long id, string orgId, int locationId, string documentType, long mainCategory,
        long subCategory, string poNumber, long vendorSiteId, long vendorId, int dueDays,
        long poId, string invoiceNo, long invoiceAmount, int currency, DateTime invoiceDate,
        DateTime invoiceReceiptDate, long pages, DateTime paymentDueDate, int payBy,
        long owner, string documentStatus, long userId, DateTime createdOn,
        DateTime submittedOn, long receivedBy, DateTime receivedOn, string accountCode)
    {
        var doc = new DocumentDetail
        {
            Id = id,
            OrgId = orgId,
            LocationId = locationId,
            DocumentType = documentType,
            MainCategory = mainCategory,
            SubCategory = subCategory,
            PoNumber = poNumber,
            VendorSiteId = vendorSiteId,
            VendorId = vendorId,
            DueDays = dueDays,
            PoId = poId,
            InvoiceNo = invoiceNo,
            InvoiceAmount = invoiceAmount,
            Currency = currency,
            InvoiceDate = invoiceDate,
            InvoiceReceiptDate = invoiceReceiptDate,
            Pages = pages,
            PaymentDueDate = paymentDueDate,
            PayBy = payBy,
            Owner = owner,
            DocumentStatus = documentStatus,
            UserId = userId,
            CreatedOn = createdOn,
            SubmittedOn = submittedOn,
            ReceivedBy = receivedBy,
            ReceivedOn = receivedOn,
            AccountCode = accountCode,
            CancelFlag = "N"
        };

        doc.AddDomainEvent(new DocumentCreatedEvent(doc.Id, doc.OrgId, doc.DocumentType));
        return doc;
    }

    public void Submit()
    {
        DocumentStatus = "SB";
        SubmittedOn = DateTime.UtcNow;
        AddDomainEvent(new DocumentSubmittedEvent(Id, OrgId));
    }

    public void Approve(long approvedBy)
    {
        DocumentStatus = "AP";
        ApprovedBy = approvedBy;
        AddDomainEvent(new DocumentApprovedEvent(Id, approvedBy));
    }

    public void Cancel(long cancelUser, string? remarks)
    {
        CancelFlag = "Y";
        CancelUser = cancelUser;
        CancelDate = DateTime.UtcNow;
        CancelRemarks = remarks;
        DocumentStatus = "CN";
        AddDomainEvent(new DocumentCancelledEvent(Id, cancelUser));
    }

    public void PutOnHold(string? holdRemarks)
    {
        HoldStatus = "Y";
        HoldPaymentFlag = "Y";
        HoldPaymentRemarks = holdRemarks;
        AddDomainEvent(new DocumentHoldEvent(Id));
    }

    public void ReleaseHold(string? releaseRemarks)
    {
        HoldStatus = "N";
        HoldPaymentFlag = "N";
        HoldReleaseRemarks = releaseRemarks;
    }

    public void UpdateInvoiceStatus(string status)
    {
        InvoiceStatus = status;
    }

    public void SetFilePath(string filePath, string? sscPdf = null, string? userPdf = null)
    {
        FilePath = filePath;
        SscInvoicePdf = sscPdf;
        UserInvoicePdf = userPdf;
    }
}
