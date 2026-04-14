using WMTransactional.Domain.Entities;

namespace WMTransactional.Domain.Aggregates;

public class SalesOrderAggregate
{
    public SalesOrder SalesOrder { get; }
    private readonly List<Shipment> _shipments = [];
    public IReadOnlyCollection<Shipment> Shipments => _shipments.AsReadOnly();

    public SalesOrderAggregate(SalesOrder salesOrder)
    {
        SalesOrder = salesOrder;
    }

    public Shipment CreateShipment(string shipmentNumber, string? trackingNumber, string? carrier, string? notes, string? createdBy)
    {
        SalesOrder.StartPicking();

        var shipment = new Shipment(shipmentNumber, SalesOrder.SoId, trackingNumber, carrier, notes, createdBy);
        _shipments.Add(shipment);
        return shipment;
    }

    public bool AreAllLinesFullyShipped()
    {
        return SalesOrder.Lines.All(l => l.IsFullyShipped);
    }

    public void CompleteIfFullyShipped()
    {
        if (AreAllLinesFullyShipped())
        {
            SalesOrder.StartShipping();
            SalesOrder.Complete();
        }
    }
}
