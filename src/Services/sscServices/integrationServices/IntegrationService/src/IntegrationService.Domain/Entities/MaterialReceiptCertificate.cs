using IntegrationService.Domain.Common;

namespace IntegrationService.Domain.Entities;

public class MaterialReceiptCertificate : BaseEntity<long>
{
    public long PurchaseOrderId { get; private set; }
    public string MrcNumber { get; private set; } = string.Empty;
    public long? SequenceNumber { get; private set; }
    public DateTime? ReceiveDate { get; private set; }
    public long? VendorId { get; private set; }
    public long? VendorSiteId { get; private set; }

    private MaterialReceiptCertificate() { }

    public static MaterialReceiptCertificate Create(long seqId, long purchaseOrderId,
        string mrcNumber, long? sequenceNumber, DateTime? receiveDate,
        long? vendorId, long? vendorSiteId)
    {
        if (string.IsNullOrWhiteSpace(mrcNumber))
            throw new ArgumentException("MRC number is required.", nameof(mrcNumber));

        return new MaterialReceiptCertificate
        {
            Id = seqId,
            PurchaseOrderId = purchaseOrderId,
            MrcNumber = mrcNumber,
            SequenceNumber = sequenceNumber,
            ReceiveDate = receiveDate,
            VendorId = vendorId,
            VendorSiteId = vendorSiteId
        };
    }

    public void UpdateReceiveDate(DateTime receiveDate)
    {
        ReceiveDate = receiveDate;
    }
}
