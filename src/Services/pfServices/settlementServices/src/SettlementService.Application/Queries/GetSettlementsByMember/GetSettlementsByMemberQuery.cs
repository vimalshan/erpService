using MediatR;
using SettlementService.Application.DTOs;

namespace SettlementService.Application.Queries.GetSettlementsByMember;

public record GetSettlementsByMemberQuery(long MemberNo) : IRequest<IEnumerable<SettlementDto>>;
