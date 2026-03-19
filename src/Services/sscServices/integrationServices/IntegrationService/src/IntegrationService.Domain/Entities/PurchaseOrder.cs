using IntegrationService.Domain.Common;
using IntegrationService.Domain.Events;
using IntegrationService.Domain.ValueObjects;

namespace IntegrationService.Domain.Entities;

public class PurchaseOrder : BaseEntity<long>, IAggregateRoot
{
    public long OracleOrgId { get; private set; }
    public long OraclePoId { get; private set; }
    public string PoNumber { get; private set; } = string.Empty;
    public long VendorSiteId { get; private set; }
    public PaymentTerms PaymentTerms { get; private set; } = null!;

    private readonly List<MaterialReceiptCertificate> _materialReceipts = [];
    public IReadOnlyCollection<MaterialReceiptCertificate> MaterialReceipts => _materialReceipts.AsReadOnly();

    private PurchaseOrder() { }

    public static PurchaseOrder Create(long seqId, long oracleOrgId, long oraclePoId,
        string poNumber, long vendorSiteId, PaymentTerms paymentTerms)
    {
        if (string.IsNullOrWhiteSpace(poNumber))
            throw new ArgumentException("PO number is required.", nameof(poNumber));

        var po = new PurchaseOrder
        {
            Id = seqId,
            OracleOrgId = oracleOrgId,
            OraclePoId = oraclePoId,
            PoNumber = poNumber,
            VendorSiteId = vendorSiteId,
            PaymentTerms = paymentTerms
        };

        po.AddDomainEvent(new PurchaseOrderCreatedEvent(po.Id, po.PoNumber));
        return po;
    }

    public void UpdatePaymentTerms(PaymentTerms paymentTerms)
    {
        PaymentTerms = paymentTerms;
        AddDomainEvent(new PurchaseOrderUpdatedEvent(Id, PoNumber));
    }

    public void AddMaterialReceipt(MaterialReceiptCertificate mrc)
    {
        _materialReceipts.Add(mrc);
        AddDomainEvent(new MaterialReceiptAddedEvent(Id, mrc.Id, mrc.MrcNumber));
    }
}
