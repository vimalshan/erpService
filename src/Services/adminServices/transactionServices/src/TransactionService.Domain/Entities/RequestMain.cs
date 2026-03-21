namespace TransactionService.Domain.Entities;

using TransactionService.Domain.Common;
using TransactionService.Domain.Events;
using TransactionService.Domain.ValueObjects;

public sealed class RequestMain : AggregateRoot
{
    private readonly List<RequestSub> _details = [];

    public long RequestId { get; private set; }
    public long RequestedBy { get; private set; }
    public DateTime RequestedOn { get; private set; }
    public long? LocationId { get; private set; }
    public UnitCode? UnitCode { get; private set; }

    public IReadOnlyCollection<RequestSub> Details => _details.AsReadOnly();

    private RequestMain() { }

    public static RequestMain Create(
        long requestId, long requestedBy, long? locationId, string? unitCode)
    {
        var request = new RequestMain
        {
            RequestId = requestId,
            RequestedBy = requestedBy,
            RequestedOn = DateTime.UtcNow,
            LocationId = locationId,
            UnitCode = string.IsNullOrWhiteSpace(unitCode) ? null : new UnitCode(unitCode)
        };

        request.RaiseDomainEvent(new RequestCreatedEvent(
            request.RequestId, request.RequestedBy, request.LocationId, DateTime.UtcNow));

        return request;
    }

    public void AddDetail(RequestSub detail)
    {
        _details.Add(detail);
    }

    public void ApproveDetail(long requestSubId, long approvedQty, long approverSysId, string? remarks)
    {
        var detail = _details.FirstOrDefault(d => d.RequestSubId == requestSubId)
            ?? throw new InvalidOperationException($"Request sub {requestSubId} not found.");

        detail.Approve(approvedQty, approverSysId, remarks);

        if (_details.All(d => d.Status.IsApproved || d.Status.IsRejected))
        {
            RaiseDomainEvent(new RequestFullyProcessedEvent(
                RequestId, RequestedBy, DateTime.UtcNow));
        }
    }
}
