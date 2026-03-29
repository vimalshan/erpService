using HealthTransaction.Domain.Common;
using HealthTransaction.Domain.Entities;

namespace HealthTransaction.Domain.Events;

public class CheckupCardCreatedEvent : BaseEvent
{
    public CheckupCardCreatedEvent(CheckupCard checkupCard) => CheckupCard = checkupCard;
    public CheckupCard CheckupCard { get; }
}
