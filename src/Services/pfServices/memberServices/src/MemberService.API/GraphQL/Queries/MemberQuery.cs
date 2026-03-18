using MediatR;
using MemberService.Application.DTOs;
using MemberService.Application.Queries.GetMember;

namespace MemberService.API.GraphQL.Queries;

[QueryType]
public class MemberQuery
{
    public async Task<MemberProfileDto?> GetMemberAsync(long memberNo, [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetMemberQuery(memberNo), ct);

    public async Task<IReadOnlyList<MemberSummaryDto>> GetMembersAsync(string? trustCode,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllMembersQuery(trustCode), ct);

    public async Task<MemberDto?> GetMemberByEmployeeAsync(long employeeSysId,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetMemberByEmployeeQuery(employeeSysId), ct);
}
