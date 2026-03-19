using HRDocumentService.Domain.Common;

namespace HRDocumentService.Domain.Entities;

public class HRDocumentReceipt : BaseEntity
{
    public long HRRecId { get; private set; }
    public long HRRecEnvId { get; private set; }
    public long HRRecHRDocId { get; private set; }
    public long HRRecUpdatedBy { get; private set; }
    public DateTime HRRecUpdatedOn { get; private set; }

    private HRDocumentReceipt() { }

    public static HRDocumentReceipt Create(long hrRecId, long hrRecEnvId, long hrRecHRDocId, long updatedBy)
    {
        return new HRDocumentReceipt
        {
            HRRecId = hrRecId,
            HRRecEnvId = hrRecEnvId,
            HRRecHRDocId = hrRecHRDocId,
            HRRecUpdatedBy = updatedBy,
            HRRecUpdatedOn = DateTime.UtcNow
        };
    }
}
