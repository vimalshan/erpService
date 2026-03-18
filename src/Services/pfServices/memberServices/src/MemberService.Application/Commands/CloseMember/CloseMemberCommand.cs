using MediatR;

namespace MemberService.Application.Commands.CloseMember;

public record CloseMemberCommand(
    long MemberNo,
    string LeaveReason,
    DateTime LeaveDate,
    long ApprovedBy
) : IRequest<bool>;
