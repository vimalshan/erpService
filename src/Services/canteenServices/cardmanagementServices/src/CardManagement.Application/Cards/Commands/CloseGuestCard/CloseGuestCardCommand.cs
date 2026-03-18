using MediatR;

namespace CardManagement.Application.Cards.Commands.CloseGuestCard;

public record CloseGuestCardCommand(long CanteenUnit) : IRequest<bool>;
