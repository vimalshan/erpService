using CategoryAndVendorService.Domain.Common;
using CategoryAndVendorService.Domain.Events;
using CategoryAndVendorService.Domain.ValueObjects;

namespace CategoryAndVendorService.Domain.Entities;

/// <summary>
/// Aggregate Root: Vendor Document Details (VENDOR_DOCDET)
/// </summary>
public class VendorDocument : Entity
{
    public long VndDocId { get; private set; }
    public long VendorId { get; private set; }
    public long SiteId { get; private set; }
    public long BuId { get; private set; }
    public long InformationCategory { get; private set; }
    public string Remarks { get; private set; } = null!;
    public char DocFlag { get; private set; }
    public long? DocType { get; private set; }
    public string? DocRefNo { get; private set; }
    public DateTime ValidFrom { get; private set; }
    public DateTime? ValidTo { get; private set; }
    public char ActiveStatus { get; private set; }
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public ApprovalStatus ApprovalStatus { get; private set; }
    public string? ApprovalRemarks { get; private set; }
    public long? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }

    private readonly List<VendorDocumentFile> _files = new();
    public IReadOnlyCollection<VendorDocumentFile> Files => _files.AsReadOnly();

    private VendorDocument() { }

    public static VendorDocument Create(long id, long vendorId, long siteId, long buId,
        long infoCat, string remarks, char docFlag, DateTime validFrom,
        long modifiedBy, long? docType = null, string? docRefNo = null, DateTime? validTo = null)
    {
        var doc = new VendorDocument
        {
            VndDocId = id,
            VendorId = vendorId,
            SiteId = siteId,
            BuId = buId,
            InformationCategory = infoCat,
            Remarks = remarks,
            DocFlag = docFlag,
            DocType = docType,
            DocRefNo = docRefNo,
            ValidFrom = validFrom,
            ValidTo = validTo,
            ActiveStatus = 'Y',
            ModifiedBy = modifiedBy,
            ModifiedOn = DateTime.UtcNow,
            ApprovalStatus = ApprovalStatus.FromCode('N')
        };
        doc.RaiseDomainEvent(new VendorDocumentCreatedEvent(id, vendorId));
        return doc;
    }

    public void Approve(long approvedBy, string? remarks = null)
    {
        ApprovalStatus = ApprovalStatus.Approved;
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
        ApprovalRemarks = remarks;
        ModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new VendorDocumentApprovedEvent(VndDocId, VendorId, approvedBy));
    }

    public void Reject(long rejectedBy, string remarks)
    {
        ApprovalStatus = ApprovalStatus.Rejected;
        ApprovedBy = rejectedBy;
        ApprovedOn = DateTime.UtcNow;
        ApprovalRemarks = remarks;
        ModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new VendorDocumentRejectedEvent(VndDocId, VendorId, rejectedBy));
    }

    public void Submit()
    {
        ApprovalStatus = ApprovalStatus.PendingApproval;
        ModifiedOn = DateTime.UtcNow;
    }

    public void Deactivate(long modifiedBy)
    {
        ActiveStatus = 'N';
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public void AddFile(VendorDocumentFile file) => _files.Add(file);
}
