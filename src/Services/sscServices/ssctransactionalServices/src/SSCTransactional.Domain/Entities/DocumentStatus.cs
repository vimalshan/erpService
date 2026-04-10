using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.Entities;

/// <summary>Maps to DOC_STATUS — Document status reference</summary>
public class DocumentStatus : Entity<string>
{
    public string DocType { get; private set; } = default!;         // D=Document, I=Invoice
    public string CompletedRemark { get; private set; } = default!;
    public string PendingRemark { get; private set; } = default!;
    public long? StageOrder { get; private set; }
    public string? CategoryGroup { get; private set; }
    public long? StageNo { get; private set; }

    private DocumentStatus() { }

    public static DocumentStatus Create(string flag, string docType, string completedRemark, string pendingRemark,
        long? stageOrder = null, string? categoryGroup = null, long? stageNo = null)
    {
        return new DocumentStatus
        {
            Id = flag,
            DocType = docType,
            CompletedRemark = completedRemark,
            PendingRemark = pendingRemark,
            StageOrder = stageOrder,
            CategoryGroup = categoryGroup,
            StageNo = stageNo
        };
    }
}
