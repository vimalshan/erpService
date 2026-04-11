using TransactionService.Domain.Common;

namespace TransactionService.Domain.Entities;

/// <summary>
/// Maps to TRAVEL_BATCHSUBBRK - Batch sub break for vendor JV
/// </summary>
public sealed class TravelBatchSubBreak : BaseEntity
{
    private TravelBatchSubBreak() { }

    public string? BatchBrkId { get; private set; }
    public string? BatchSubId { get; private set; }
    public string? VendorId { get; private set; }
    public string? VendorSiteId { get; private set; }
    public string? JvId { get; private set; }

    public static TravelBatchSubBreak Create(
        string? batchBrkId, string? batchSubId,
        string? vendorId = null, string? vendorSiteId = null, string? jvId = null)
    {
        return new TravelBatchSubBreak
        {
            BatchBrkId = batchBrkId,
            BatchSubId = batchSubId,
            VendorId = vendorId,
            VendorSiteId = vendorSiteId,
            JvId = jvId
        };
    }
}
