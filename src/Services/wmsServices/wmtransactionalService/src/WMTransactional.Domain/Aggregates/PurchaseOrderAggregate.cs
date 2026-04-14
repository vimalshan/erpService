using WMTransactional.Domain.Entities;

namespace WMTransactional.Domain.Aggregates;

public class PurchaseOrderAggregate
{
    public PurchaseOrder PurchaseOrder { get; }
    private readonly List<Receiving> _receivings = [];
    public IReadOnlyCollection<Receiving> Receivings => _receivings.AsReadOnly();

    public PurchaseOrderAggregate(PurchaseOrder purchaseOrder)
    {
        PurchaseOrder = purchaseOrder;
    }

    public Receiving CreateReceiving(string receivingNumber, string? notes, string? createdBy)
    {
        PurchaseOrder.StartReceiving();

        var receiving = new Receiving(receivingNumber, PurchaseOrder.PoId, notes, createdBy);
        _receivings.Add(receiving);
        return receiving;
    }

    public bool AreAllLinesFullyReceived()
    {
        return PurchaseOrder.Lines.All(l => l.IsFullyReceived);
    }

    public void CompleteIfFullyReceived()
    {
        if (AreAllLinesFullyReceived())
        {
            PurchaseOrder.Complete();
        }
    }
}
