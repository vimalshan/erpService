using MediatR;

namespace CardManagement.Application.Cards.Commands.UpdateGuestCard;

public record UpdateGuestCardCommand(
    long CanteenUnit,
    string? CardName,
    string? CardType,
    string? ReportingUnit,
    decimal? ReportingDepartment
) : IRequest<bool>;
