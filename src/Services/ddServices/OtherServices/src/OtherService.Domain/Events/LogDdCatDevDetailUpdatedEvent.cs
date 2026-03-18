using OtherService.Domain.Common;
using OtherService.Domain.Entities;

namespace OtherService.Domain.Events;

public sealed class LogDdCatDevDetailUpdatedEvent : DomainEvent
{
    public LogDdCatDevDetailUpdatedEvent(LogDdCatDevDetail entity)
    {
        Entity = entity;
    }

    public LogDdCatDevDetail Entity { get; }
}
