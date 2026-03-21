using ArchiveService.Domain.Common;
using ArchiveService.Domain.Events;
using ArchiveService.Domain.ValueObjects;

namespace ArchiveService.Domain.Entities;

public class ArchivedServiceOrder : AggregateRoot<string>
{
    public string SernoDell { get; private set; } = string.Empty;
    public string? Branch { get; private set; }
    public string? SapLogin { get; private set; }
    public DateTime? PostingDate { get; private set; }
    public string? SapId { get; private set; }
    public string? Sla { get; private set; }
    public string? ProductId { get; private set; }
    public string? ServiceTag { get; private set; }
    public string? RelatedCase { get; private set; }
    public string? Lob { get; private set; }
    public string? CallStatus { get; private set; }
    public string? CurrentRc { get; private set; }
    public EngineerInfo Engineer { get; private set; } = new(null, null, null);
    public string? OrgName { get; private set; }
    public string? CustomerName { get; private set; }
    public ContactInfo Contact { get; private set; } = new(null, null);
    public Address? Address { get; private set; }
    public DateTime? DispatchDate { get; private set; }
    public DateTime? CustEtaDate { get; private set; }
    public DateTime? PartEtaDate { get; private set; }
    public string? TechSupName { get; private set; }
    public string? Dsp { get; private set; }
    public string? ProblemDescription { get; private set; }
    public string? LongDescription { get; private set; }
    public string? ReasonCode { get; private set; }
    public string? Activity { get; private set; }
    public DateTime? OnsiteDate { get; private set; }
    public DateTime? CompletedDate { get; private set; }
    public string? Flag { get; private set; }

    public ICollection<ArchivedServiceOrderDetail> Details { get; private set; } = new List<ArchivedServiceOrderDetail>();

    private ArchivedServiceOrder() { }

    public static ArchivedServiceOrder Create(
        string sernoDell, string? branch, string? sapLogin, DateTime? postingDate,
        string? sapId, string? sla, string? productId, string? serviceTag,
        string? relatedCase, string? lob, string? callStatus, string? currentRc,
        string? engineerId, string? engineerName, string? engMobNo,
        string? orgName, string? customerName, string? contactNo,
        string? address, string? altCntNo, DateTime? dispatchDate,
        DateTime? custEtaDate, DateTime? partEtaDate, string? techSupName,
        string? dsp, string? prbDesc, string? longDesc, string? reasonCode,
        string? activity, DateTime? onsiteDt, DateTime? cmpltdDt,
        string? flag, string? enteredBy)
    {
        var order = new ArchivedServiceOrder
        {
            Id = sernoDell,
            SernoDell = sernoDell,
            Branch = branch,
            SapLogin = sapLogin,
            PostingDate = postingDate,
            SapId = sapId,
            Sla = sla,
            ProductId = productId,
            ServiceTag = serviceTag,
            RelatedCase = relatedCase,
            Lob = lob,
            CallStatus = callStatus,
            CurrentRc = currentRc,
            Engineer = new EngineerInfo(engineerId, engineerName, engMobNo),
            OrgName = orgName,
            CustomerName = customerName,
            Contact = new ContactInfo(contactNo, altCntNo),
            Address = address != null ? new Address(address) : null,
            DispatchDate = dispatchDate,
            CustEtaDate = custEtaDate,
            PartEtaDate = partEtaDate,
            TechSupName = techSupName,
            Dsp = dsp,
            ProblemDescription = prbDesc,
            LongDescription = longDesc,
            ReasonCode = reasonCode,
            Activity = activity,
            OnsiteDate = onsiteDt,
            CompletedDate = cmpltdDt,
            Flag = flag,
            EnteredOn = DateTime.UtcNow,
            EnteredBy = enteredBy
        };

        order.AddDomainEvent(new ServiceOrderArchivedEvent(sernoDell, sapId));
        return order;
    }

    public void UpdateStatus(string? callStatus, string? reasonCode, string? changedBy)
    {
        CallStatus = callStatus;
        ReasonCode = reasonCode;
        ChangedOn = DateTime.UtcNow;
        ChangedBy = changedBy;

        AddDomainEvent(new ServiceOrderStatusChangedEvent(SernoDell, callStatus));
    }

    public void AddDetail(ArchivedServiceOrderDetail detail)
    {
        Details.Add(detail);
    }
}
