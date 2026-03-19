namespace OrderScheduleService.Domain.Aggregates;

using OrderScheduleService.Domain.Common;
using OrderScheduleService.Domain.Entities;
using OrderScheduleService.Domain.Events;

public class ScheduleAggregate : AggregateRoot
{
    public long FillingPointGroupId { get; private set; }
    public decimal ItemId { get; private set; }
    public string OrderType { get; private set; } = null!;
    public long OrderId { get; private set; }
    public long OrderLineId { get; private set; }
    public DateTime RequiredDate { get; private set; }
    public decimal OrderQuantity { get; private set; }
    public decimal ShiftCapacity { get; private set; }
    public decimal TotalAllocatedQuantity { get; private set; }

    private readonly List<ScheduleDetail> _scheduleDetails = new();
    public IReadOnlyCollection<ScheduleDetail> ScheduleDetails => _scheduleDetails.AsReadOnly();

    public ScheduleAggregate() { }

    public ScheduleAggregate(
        long scheduleId,
        long fillingPointGroupId,
        decimal itemId,
        string orderType,
        long orderId,
        long orderLineId,
        DateTime requiredDate,
        decimal orderQuantity,
        decimal shiftCapacity)
    {
        Id = scheduleId;
        FillingPointGroupId = fillingPointGroupId;
        ItemId = itemId;
        OrderType = orderType;
        OrderId = orderId;
        OrderLineId = orderLineId;
        RequiredDate = requiredDate;
        OrderQuantity = orderQuantity;
        ShiftCapacity = shiftCapacity;
        TotalAllocatedQuantity = 0;
    }

    public void AddScheduleDetail(
        DateTime fillingDate,
        char fillingShift,
        string startTime,
        string endTime,
        decimal fillQuantity,
        long fillingPointGroupId)
    {
        if (fillQuantity <= 0)
            throw new InvalidOperationException("Fill quantity must be greater than zero");

        var detail = new ScheduleDetail(
            Id,
            fillingDate,
            fillingShift,
            startTime,
            endTime,
            fillQuantity,
            (decimal)fillingPointGroupId);

        _scheduleDetails.Add(detail);
        TotalAllocatedQuantity += fillQuantity;

        AddDomainEvent(new OrderScheduledEvent(
            OrderId,
            OrderLineId,
            fillingDate,
            (long)fillQuantity));
    }

    public void ConfirmSchedule()
    {
        AddDomainEvent(new ScheduleConfirmedEvent(
            RequiredDate,
            "CONFIRMED"));
    }

    public decimal GetRemainingCapacity() => ShiftCapacity - TotalAllocatedQuantity;

    public bool CanAllocateQuantity(decimal quantity) => GetRemainingCapacity() >= quantity;
}
