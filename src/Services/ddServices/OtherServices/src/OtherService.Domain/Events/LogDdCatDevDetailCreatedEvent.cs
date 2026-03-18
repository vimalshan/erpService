using OtherService.Domain.Common;
using OtherService.Domain.Entities;

namespace OtherService.Domain.Events;

public sealed class LogDdCatDevDetailCreatedEvent : DomainEvent
{
    public LogDdCatDevDetailCreatedEvent(LogDdCatDevDetail entity)
    {
        Entity = entity;
    }

    public LogDdCatDevDetail Entity { get; }
}
