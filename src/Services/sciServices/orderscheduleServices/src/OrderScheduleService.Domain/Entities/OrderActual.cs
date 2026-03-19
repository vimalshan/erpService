namespace OrderScheduleService.Domain.Entities;

using OrderScheduleService.Domain.Common;

public class OrderActual : Entity
{
    public decimal OrderNumber { get; set; }
    public decimal HeaderId { get; set; }
    public decimal LineId { get; set; }
    public string? OrderedItem { get; set; }
    public decimal? OrderedItemId { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? ScheduleShipDate { get; set; }
    public DateTime? ActualShipmentDate { get; set; }
    public decimal? OrderedQuantity { get; set; }
    public string? OrderQuantityUom { get; set; }
    public decimal? CancelledQuantity { get; set; }
    public decimal? FulfilledQuantity { get; set; }
    public decimal? ShippingQuantity { get; set; }
    public string? ShippingQuantityUom { get; set; }
    public decimal? InvoicedQuantity { get; set; }
    public decimal? ShipFromOrgId { get; set; }
    public decimal? SoldFromOrgId { get; set; }
    public decimal? SoldToOrgId { get; set; }
    public string? CustomerName { get; set; }
    public decimal? ShipToOrgId { get; set; }
    public string? ConsigneeName { get; set; }
    public string? CustPoNumber { get; set; }
    public DateTime? OrderedDate { get; set; }
    public decimal? OrderSourceId { get; set; }

    public OrderActual() { }

    public OrderActual(
        decimal orderNumber,
        decimal lineId,
        decimal? orderedQuantity,
        DateTime? requestDate,
        DateTime? scheduleShipDate,
        string? orderedItem = null)
    {
        OrderNumber = orderNumber;
        LineId = lineId;
        OrderedQuantity = orderedQuantity;
        RequestDate = requestDate;
        ScheduleShipDate = scheduleShipDate;
        OrderedItem = orderedItem;
    }

    public void Fulfill(decimal fulfilledQuantity)
    {
        FulfilledQuantity = fulfilledQuantity;
    }

    public void Ship(decimal shippingQuantity, DateTime actualShipmentDate)
    {
        ShippingQuantity = shippingQuantity;
        ActualShipmentDate = actualShipmentDate;
    }
}
