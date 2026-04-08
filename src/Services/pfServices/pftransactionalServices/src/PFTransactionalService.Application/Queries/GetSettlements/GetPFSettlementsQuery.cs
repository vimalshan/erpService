using MediatR;
using PFTransactionalService.Application.DTOs;

namespace PFTransactionalService.Application.Queries.GetSettlements;

public record GetPFSettlementsQuery : IRequest<IEnumerable<PFSettlementDto>>;

public record GetPFSettlementsByEmpQuery(long EmpSysId) : IRequest<IEnumerable<PFSettlementDto>>;
