using MediatR;
using CardManagement.Application.Common.DTOs;

namespace CardManagement.Application.Cards.Commands.CreateGuestCard;

public record CreateGuestCardCommand(
    long CanteenUnit,
    long CardSequence,
    string CardNumber,
    string CardName,
    string? CardType,
    string? ReportingUnit,
    decimal? ReportingDepartment,
    DateTime EffectiveDate
) : IRequest<GuestCardDto>;
