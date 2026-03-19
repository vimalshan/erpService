using HRDocumentService.Domain.Common;
using HRDocumentService.Domain.Events;
using HRDocumentService.Domain.ValueObjects;

namespace HRDocumentService.Domain.Entities;

public class HRDocument : AggregateRoot
{
    public long DocId { get; private set; }
    public long DocNo { get; private set; }
    public string DocType { get; private set; } = null!;
    public long DocPayRefNo { get; private set; }
    public long DocLocId { get; private set; }
    public long DocUnitId { get; private set; }
    public string DocRemarks { get; private set; } = null!;
    public long DocUserId { get; private set; }
    public string? DocRefNo { get; private set; }
    public string? DocRefName { get; private set; }
    public DateTime DocCreatedOn { get; private set; }
    public string DocDocStatus { get; private set; } = null!;
    public string DocSource { get; private set; } = null!;
    public string? DocActionStatus { get; private set; }
    public DateTime? DocActionTakenOn { get; private set; }
    public decimal? DocActionTakenBy { get; private set; }
    public string? DocFilePath { get; private set; }
    public string? DocCancelFlag { get; private set; }
    public decimal? DocCancelBy { get; private set; }
    public DateTime? DocCancelOn { get; private set; }
    public decimal? DocPayBy { get; private set; }
    public string? DocRejectRemarks { get; private set; }

    private readonly List<HRDocumentFile> _files = [];
    public IReadOnlyCollection<HRDocumentFile> Files => _files.AsReadOnly();

    private readonly List<HRDocumentReceipt> _receipts = [];
    public IReadOnlyCollection<HRDocumentReceipt> Receipts => _receipts.AsReadOnly();

    private HRDocument() { }

    public static HRDocument Create(
        long docId,
        long docNo,
        DocumentType docType,
        long docPayRefNo,
        long docLocId,
        long docUnitId,
        string docRemarks,
        long docUserId,
        DocumentSource docSource,
        string? docRefNo = null,
        string? docRefName = null)
    {
        var document = new HRDocument
        {
            DocId = docId,
            DocNo = docNo,
            DocType = docType.Value,
            DocPayRefNo = docPayRefNo,
            DocLocId = docLocId,
            DocUnitId = docUnitId,
            DocRemarks = docRemarks,
            DocUserId = docUserId,
            DocRefNo = docRefNo,
            DocRefName = docRefName,
            DocCreatedOn = DateTime.UtcNow,
            DocDocStatus = DocumentStatus.Draft.Value,
            DocSource = docSource.Value
        };

        document.AddDomainEvent(new DocumentCreatedEvent(document.DocId, document.DocNo, document.DocType));
        return document;
    }

    public void Submit()
    {
        if (DocDocStatus != DocumentStatus.Draft.Value)
            throw new InvalidOperationException("Only draft documents can be submitted.");

        DocDocStatus = DocumentStatus.Submitted.Value;
        AddDomainEvent(new DocumentSubmittedEvent(DocId, DocNo));
    }

    public void Approve(decimal approvedBy)
    {
        if (DocDocStatus != DocumentStatus.Submitted.Value)
            throw new InvalidOperationException("Only submitted documents can be approved.");

        DocDocStatus = DocumentStatus.Approved.Value;
        DocActionStatus = "A";
        DocActionTakenBy = approvedBy;
        DocActionTakenOn = DateTime.UtcNow;

        AddDomainEvent(new DocumentApprovedEvent(DocId, DocNo, approvedBy));
    }

    public void Reject(decimal rejectedBy, string rejectRemarks)
    {
        if (DocDocStatus != DocumentStatus.Submitted.Value)
            throw new InvalidOperationException("Only submitted documents can be rejected.");

        DocDocStatus = DocumentStatus.Rejected.Value;
        DocActionStatus = "R";
        DocActionTakenBy = rejectedBy;
        DocActionTakenOn = DateTime.UtcNow;
        DocRejectRemarks = rejectRemarks;

        AddDomainEvent(new DocumentRejectedEvent(DocId, DocNo, rejectedBy, rejectRemarks));
    }

    public void Cancel(decimal cancelledBy)
    {
        if (DocDocStatus == DocumentStatus.Cancelled.Value)
            throw new InvalidOperationException("Document is already cancelled.");

        DocCancelFlag = "Y";
        DocCancelBy = cancelledBy;
        DocCancelOn = DateTime.UtcNow;
        DocDocStatus = DocumentStatus.Cancelled.Value;

        AddDomainEvent(new DocumentCancelledEvent(DocId, DocNo, cancelledBy));
    }

    public void MarkAsPaid(decimal paidBy)
    {
        if (DocDocStatus != DocumentStatus.Approved.Value)
            throw new InvalidOperationException("Only approved documents can be marked as paid.");

        DocDocStatus = DocumentStatus.Paid.Value;
        DocPayBy = paidBy;
    }

    public void UpdateRemarks(string remarks)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(remarks);
        DocRemarks = remarks;
    }

    public void SetFilePath(string filePath)
    {
        DocFilePath = filePath;
    }

    public void AddFile(HRDocumentFile file) => _files.Add(file);
    public void AddReceipt(HRDocumentReceipt receipt) => _receipts.Add(receipt);
}
