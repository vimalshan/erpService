using System;
using System.Collections.Generic;

namespace LocationService.Domain.Entities
{
    /// <summary>
    /// Base class for all domain entities with common functionality
    /// </summary>
    public abstract class Entity
    {
        public virtual long Id { get; protected set; }

        public DateTime CreatedOn { get; protected set; } = DateTime.UtcNow;
        public long CreatedBy { get; protected set; }

        public DateTime? UpdatedOn { get; protected set; }
        public long? UpdatedBy { get; protected set; }

        private readonly List<DomainEvent> _domainEvents = new();

        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void RaiseDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public override bool Equals(object? obj)
        {
            var compareTo = obj as Entity;
            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo?.Id == null || compareTo?.Id == 0) return false;
            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b) => !(a == b);

        public override int GetHashCode() => Id.GetHashCode();
    }

    /// <summary>
    /// Base class for domain events
    /// </summary>
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
        public Guid EventId { get; private set; } = Guid.NewGuid();
    }
}
