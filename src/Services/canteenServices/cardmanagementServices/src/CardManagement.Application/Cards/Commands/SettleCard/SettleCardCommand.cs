using MediatR;
using CardManagement.Application.Common.DTOs;

namespace CardManagement.Application.Cards.Commands.SettleCard;

public record SettleCardCommand(
    decimal SysId,
    long CanteenUnit,
    string CardNumber,
    DateTime SettlementDate
) : IRequest<CardSettlementDto>;
