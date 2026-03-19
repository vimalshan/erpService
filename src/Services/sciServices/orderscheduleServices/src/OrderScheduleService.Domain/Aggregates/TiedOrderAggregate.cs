namespace OrderScheduleService.Domain.Aggregates;

using OrderScheduleService.Domain.Common;
using OrderScheduleService.Domain.Entities;
using OrderScheduleService.Domain.Events;

public class TiedOrderAggregate : AggregateRoot
{
    public string CustomerCode { get; private set; } = null!;
    public DateTime OrderedDate { get; private set; }
    public decimal CompanyUnitId { get; private set; }
    public char RecordStatus { get; private set; }
    public string? ModifiedSciUserId { get; private set; }
    public DateTime? ModifiedDate { get; private set; }
    public string? OrderNumberCode { get; private set; }
    public string? LcNumber { get; private set; }
    
    private readonly List<OrderDetail> _details = new();
    public IReadOnlyCollection<OrderDetail> Details => _details.AsReadOnly();

    public TiedOrderAggregate() { }

    public TiedOrderAggregate(
        long orderId,
        string customerCode,
        decimal companyUnitId,
        DateTime orderedDate,
        string modifiedUserId)
    {
        Id = orderId;
        CustomerCode = customerCode;
        CompanyUnitId = companyUnitId;
        OrderedDate = orderedDate;
        ModifiedSciUserId = modifiedUserId;
        ModifiedDate = DateTime.UtcNow;
        RecordStatus = 'N';

        AddDomainEvent(new OrderCreatedEvent(
            orderId, 
            customerCode, 
            companyUnitId, 
            orderedDate));
    }

    public void AddDetail(
        decimal itemId,
        string itemName,
        long orderQuantity,
        DateTime? dispatchDate = null,
        decimal? price = null)
    {
        var detail = new OrderDetail(
            Id,
            itemId,
            itemName,
            orderQuantity,
            dispatchDate,
            price);
        
        _details.Add(detail);
        
        AddDomainEvent(new OrderDetailAddedEvent(
            Id,
            detail.Id,
            itemId,
            orderQuantity));
    }

    public void ScheduleDetail(long detailId, DateTime scheduledDate, long allocatedQuantity, int userId)
    {
        var detail = _details.FirstOrDefault(d => d.Id == detailId);
        if (detail == null)
            throw new InvalidOperationException($"Detail {detailId} not found");

        detail.AllocateFilling(allocatedQuantity, userId);
        
        AddDomainEvent(new OrderScheduledEvent(
            Id,
            detailId,
            scheduledDate,
            allocatedQuantity));
    }

    public void CancelDetail(long detailId, int userId)
    {
        var detail = _details.FirstOrDefault(d => d.Id == detailId);
        if (detail == null)
            throw new InvalidOperationException($"Detail {detailId} not found");

        detail.Cancel(userId);
        
        AddDomainEvent(new OrderCancelledEvent(
            Id,
            detailId,
            "User requested cancellation"));
    }

    public void UpdateStatus(char status, string userId)
    {
        RecordStatus = status;
        ModifiedSciUserId = userId;
        ModifiedDate = DateTime.UtcNow;
    }
}
