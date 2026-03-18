using MediatR;
using MemberService.Application.DTOs;

namespace MemberService.Application.Queries.GetMember;

public record GetMemberQuery(long MemberNo) : IRequest<MemberProfileDto?>;

public record GetMemberByEmployeeQuery(long EmployeeSysId) : IRequest<MemberDto?>;

public record GetAllMembersQuery(string? TrustCode = null, string? Status = null)
    : IRequest<IReadOnlyList<MemberSummaryDto>>;
