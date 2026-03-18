using System;

namespace AppraisalService.Domain.Events;

/// <summary>
/// Base domain event for appraisal
/// </summary>
public abstract class AppraisalDomainEvent : DomainEvent
{
    public long RequestNumber { get; protected set; }
    public string UserCode { get; protected set; }

    protected AppraisalDomainEvent(long requestNumber, string userCode)
    {
        RequestNumber = requestNumber;
        UserCode = userCode;
    }
}

public class AppraisalInitiatedDomainEvent : AppraisalDomainEvent
{
    public AppraisalInitiatedDomainEvent(long requestNumber, string userCode)
        : base(requestNumber, userCode)
    {
    }
}

public class AppraisalSubmittedByAppraiseeEvent : AppraisalDomainEvent
{
    public AppraisalSubmittedByAppraiseeEvent(long requestNumber, string userCode)
        : base(requestNumber, userCode)
    {
    }
}

public class AppraisalAssignedToAppraiserEvent : AppraisalDomainEvent
{
    public AppraisalAssignedToAppraiserEvent(long requestNumber, string userCode)
        : base(requestNumber, userCode)
    {
    }
}

public class AppraisalSubmittedByAppraiserEvent : AppraisalDomainEvent
{
    public AppraisalSubmittedByAppraiserEvent(long requestNumber, string userCode)
        : base(requestNumber, userCode)
    {
    }
}

public class AppraisalApprovedEvent : AppraisalDomainEvent
{
    public AppraisalApprovedEvent(long requestNumber, string userCode)
        : base(requestNumber, userCode)
    {
    }
}

public class AppraisalCancelledEvent : AppraisalDomainEvent
{
    public string CancellationRemarks { get; }

    public AppraisalCancelledEvent(long requestNumber, string userCode, string cancellationRemarks)
        : base(requestNumber, userCode)
    {
        CancellationRemarks = cancellationRemarks;
    }
}

public class CompensationUpdatedEvent : AppraisalDomainEvent
{
    public decimal? IncrementAmount { get; }
    public decimal? NewBasic { get; }

    public CompensationUpdatedEvent(long requestNumber, string userCode, decimal? incrementAmount, decimal? newBasic)
        : base(requestNumber, userCode)
    {
        IncrementAmount = incrementAmount;
        NewBasic = newBasic;
    }
}
