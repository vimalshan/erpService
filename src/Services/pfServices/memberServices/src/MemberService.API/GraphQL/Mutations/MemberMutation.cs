using MediatR;
using MemberService.Application.Commands.AddNominee;
using MemberService.Application.Commands.CloseMember;
using MemberService.Application.Commands.CreateMember;
using MemberService.Application.DTOs;

namespace MemberService.API.GraphQL.Mutations;

[MutationType]
public class MemberMutation
{
    public async Task<MemberDto> CreateMemberAsync(CreateMemberCommand input,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<bool> CloseMemberAsync(long memberNo, string leaveReason,
        DateTime leaveDate, long approvedBy, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new CloseMemberCommand(memberNo, leaveReason, leaveDate, approvedBy), ct);

    public async Task<NomineeDto> AddNomineeAsync(AddNomineeCommand input,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);
}
