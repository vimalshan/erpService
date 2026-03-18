namespace MemberService.Infrastructure.Messaging.Events;

public record MemberCreatedMessage(long MemberNo, string MemberName, string TrustCode, DateTime OccurredOn);
public record MemberClosedMessage(long MemberNo, string LeaveReason, DateTime LeaveDate, DateTime OccurredOn);
public record NomineeAddedMessage(long MemberNo, int SerialNo, string NomineeName, long Percentage, string FundType, DateTime OccurredOn);
