using ArchiveService.Domain.Common;

namespace ArchiveService.Domain.Entities;

public class ArchivedToolKitTransaction : BaseEntity
{
    public long Id { get; private set; }
    public long? ToolkitId { get; private set; }
    public int? ToolkitNameId { get; private set; }
    public string? EngineerId { get; private set; }
    public string? IssuerId { get; private set; }
    public int? Quantity { get; private set; }
    public string? Status { get; private set; }
    public string? Remarks { get; private set; }
    public string? AdditionalRemarks { get; private set; }

    private ArchivedToolKitTransaction() { }

    public static ArchivedToolKitTransaction Create(
        long? toolkitId, int? toolkitNameId, string? engineerId,
        string? issuerId, int? quantity, string? status,
        string? remarks, string? additionalRemarks, string? enteredBy)
    {
        return new ArchivedToolKitTransaction
        {
            ToolkitId = toolkitId,
            ToolkitNameId = toolkitNameId,
            EngineerId = engineerId,
            IssuerId = issuerId,
            Quantity = quantity,
            Status = status,
            Remarks = remarks,
            AdditionalRemarks = additionalRemarks,
            EnteredOn = DateTime.UtcNow,
            EnteredBy = enteredBy
        };
    }
}
