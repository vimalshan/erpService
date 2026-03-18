using MediatR;
using SettlementService.Application.DTOs;

namespace SettlementService.Application.Queries.GetSettlements;

public record GetSettlementsQuery : IRequest<IEnumerable<SettlementDto>>;
