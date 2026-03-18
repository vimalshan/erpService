namespace FinyearAPI.Domain.Entities
{
    /// <summary>
    /// Base Entity class for all aggregates in the domain
    /// Implements DDD entity pattern with domain events
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Unique identifier for the entity
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        /// Domain events that have occurred on this entity
        /// </summary>
        private List<DomainEvent> _domainEvents = new();

        /// <summary>
        /// Get all domain events
        /// </summary>
        public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Add a domain event
        /// </summary>
        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clear all domain events (after publishing)
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        /// <summary>
        /// Equality comparison based on ID
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var entity = (Entity)obj;
            return entity.Id == Id;
        }

        /// <summary>
        /// Hash code based on ID
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// String representation of entity
        /// </summary>
        public override string ToString()
        {
            return $"{GetType().Name} [Id: {Id}]";
        }

        // Type-safe equality operators
        public static bool operator ==(Entity? left, Entity? right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Entity? left, Entity? right)
        {
            return !(left == right);
        }
    }

    /// <summary>
    /// Base class for domain events
    /// </summary>
    public abstract class DomainEvent
    {
        /// <summary>
        /// When the event occurred
        /// </summary>
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Unique event ID for tracking
        /// </summary>
        public Guid EventId { get; set; } = Guid.NewGuid();
    }
}
