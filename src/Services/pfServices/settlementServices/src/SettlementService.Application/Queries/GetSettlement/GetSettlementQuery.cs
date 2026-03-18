using MediatR;
using SettlementService.Application.DTOs;

namespace SettlementService.Application.Queries.GetSettlement;

public record GetSettlementQuery(long SettlementNumber) : IRequest<SettlementDto?>;
