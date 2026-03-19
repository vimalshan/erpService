using System;

#nullable enable

namespace MasterData.Domain.Events
{
    /// <summary>
    /// Base class for all domain events
    /// </summary>
    public abstract record DomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredAt { get; } = DateTime.UtcNow;
    }

    // Company Unit Domain Events
    public record CompanyUnitCreatedEvent(string Code, string Name) : DomainEvent;

    public record CompanyUnitUpdatedEvent(int Id, string Code, string Name) : DomainEvent;

    public record CompanyUnitDeletedEvent(int Id) : DomainEvent;

    // Location Domain Events
    public record LocationCreatedEvent(string Name) : DomainEvent;

    public record LocationUpdatedEvent(int Id, string Name) : DomainEvent;

    public record LocationDeletedEvent(int Id) : DomainEvent;

    // Supplier Domain Events
    public record SupplierCreatedEvent(string Code, string Name, string? Details) : DomainEvent;

    public record SupplierUpdatedEvent(string Code, string Name, string? Details) : DomainEvent;

    public record SupplierDeletedEvent(string Code) : DomainEvent;

    // State Domain Events
    public record StateCreatedEvent(string Code, string Name) : DomainEvent;

    public record StateUpdatedEvent(string Code, string Name) : DomainEvent;

    public record StateDeletedEvent(string Code) : DomainEvent;

    // City Domain Events
    public record CityCreatedEvent(string Code, string Name, string StateCode) : DomainEvent;

    public record CityUpdatedEvent(string Code, string Name, string StateCode) : DomainEvent;

    public record CityDeletedEvent(string Code) : DomainEvent;
}
