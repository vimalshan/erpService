using VisitorServices.Domain.Common;
using VisitorServices.Domain.Enums;
using VisitorServices.Domain.Events;
using VisitorServices.Domain.ValueObjects;

namespace VisitorServices.Domain.Aggregates;

/// <summary>
/// Aggregate root for the Visitor bounded context.
/// Maps to VISITOR_MAIN and owns VISITOR_ITEM and VISITOR_APPREQUEST child collections.
/// </summary>
public sealed class VisitorAggregate : AggregateRoot
{
    private readonly List<Entities.VisitorItem> _items = [];
    private readonly List<Entities.VisitorApprovalRequest> _approvalRequests = [];

    public string Name { get; private set; } = string.Empty;
    public IdDocument IdDocument { get; private set; } = null!;
    public ContactInfo ContactInfo { get; private set; } = null!;
    public string? Company { get; private set; }
    public string? Purpose { get; private set; }
    public DateTime CheckInTime { get; private set; }
    public DateTime? CheckOutTime { get; private set; }
    public VisitorStatus Status { get; private set; }
    public long WhomToVisit { get; private set; }
    public DateTime EnteredOn { get; private set; }
    public long EnteredBy { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public IReadOnlyCollection<Entities.VisitorItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<Entities.VisitorApprovalRequest> ApprovalRequests => _approvalRequests.AsReadOnly();

    private VisitorAggregate() { }

    public static VisitorAggregate Register(
        long id,
        string name,
        char idTypeChar,
        string? idNumber,
        string? phone,
        string? email,
        string? company,
        string? purpose,
        long whomToVisit,
        long enteredBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Visitor name is required.", nameof(name));

        var visitor = new VisitorAggregate
        {
            Id = id,
            Name = name.Trim(),
            IdDocument = IdDocument.Create(idTypeChar, idNumber),
            ContactInfo = new ContactInfo(phone, email),
            Company = company?.Trim(),
            Purpose = purpose?.Trim(),
            CheckInTime = DateTime.UtcNow,
            Status = VisitorStatus.Inside,
            WhomToVisit = whomToVisit,
            EnteredOn = DateTime.UtcNow,
            EnteredBy = enteredBy,
            LastModifiedBy = enteredBy,
            LastModifiedOn = DateTime.UtcNow
        };

        visitor.AddDomainEvent(new VisitorRegisteredEvent(id, name.Trim(), enteredBy));
        return visitor;
    }

    public void Checkout(long checkedOutBy)
    {
        if (Status != VisitorStatus.Inside)
            throw new InvalidOperationException("Only visitors currently inside can be checked out.");

        CheckOutTime = DateTime.UtcNow;
        Status = VisitorStatus.Outside;
        LastModifiedBy = checkedOutBy;
        LastModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new VisitorCheckedOutEvent(Id, CheckOutTime.Value, checkedOutBy));
    }

    public Entities.VisitorItem AddItem(
        long itemId,
        string description,
        int quantity,
        string? materialType,
        string? notes,
        long enteredBy)
    {
        var item = Entities.VisitorItem.Create(itemId, Id, description, quantity, materialType, notes, enteredBy);
        _items.Add(item);
        return item;
    }

    public Entities.VisitorApprovalRequest RequestApproval(
        long requestId,
        long approverId,
        long requestedBy)
    {
        var request = Entities.VisitorApprovalRequest.Create(requestId, Id, approverId, requestedBy);
        _approvalRequests.Add(request);
        AddDomainEvent(new ApprovalRequestedEvent(requestId, Id, approverId, requestedBy));
        return request;
    }

    // EF Core navigation hydration
    public void HydrateItems(IEnumerable<Entities.VisitorItem> items) =>
        _items.AddRange(items);

    public void HydrateApprovalRequests(IEnumerable<Entities.VisitorApprovalRequest> requests) =>
        _approvalRequests.AddRange(requests);
}
