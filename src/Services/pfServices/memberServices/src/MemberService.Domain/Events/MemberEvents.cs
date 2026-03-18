using MemberService.Domain.Common;

namespace MemberService.Domain.Events;

public sealed record MemberCreatedEvent(
    Guid EventId,
    DateTime OccurredOn,
    long MemberNo,
    string MemberName,
    string TrustCode,
    long CreatedBy
) : IDomainEvent
{
    public string EventType => nameof(MemberCreatedEvent);

    public static MemberCreatedEvent Create(long memberNo, string memberName, string trustCode, long createdBy) =>
        new(Guid.NewGuid(), DateTime.UtcNow, memberNo, memberName, trustCode, createdBy);
}

public sealed record MemberClosedEvent(
    Guid EventId,
    DateTime OccurredOn,
    long MemberNo,
    string LeaveReason,
    DateTime LeaveDate,
    long ApprovedBy
) : IDomainEvent
{
    public string EventType => nameof(MemberClosedEvent);

    public static MemberClosedEvent Create(long memberNo, string leaveReason, DateTime leaveDate, long approvedBy) =>
        new(Guid.NewGuid(), DateTime.UtcNow, memberNo, leaveReason, leaveDate, approvedBy);
}

public sealed record NomineeAddedEvent(
    Guid EventId,
    DateTime OccurredOn,
    long MemberNo,
    int SerialNo,
    string NomineeName,
    long Percentage,
    string FundType
) : IDomainEvent
{
    public string EventType => nameof(NomineeAddedEvent);

    public static NomineeAddedEvent Create(long memberNo, int serialNo, string nomineeName, long percentage, string fundType) =>
        new(Guid.NewGuid(), DateTime.UtcNow, memberNo, serialNo, nomineeName, percentage, fundType);
}

public sealed record MemberStatusChangedEvent(
    Guid EventId,
    DateTime OccurredOn,
    long MemberNo,
    string OldStatus,
    string NewStatus,
    long ChangedBy
) : IDomainEvent
{
    public string EventType => nameof(MemberStatusChangedEvent);
}
