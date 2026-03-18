using MediatR;
using System;
using System.Collections.Generic;

namespace EmployeeService.Domain.Common
{
    /// <summary>
    /// Base entity class for all domain entities
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Entity created date
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// User who created the entity
        /// </summary>
        public long? CreatedBy { get; set; }

        /// <summary>
        /// Entity last modified date
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// User who last modified the entity
        /// </summary>
        public long? ModifiedBy { get; set; }

        /// <summary>
        /// Is entity deleted (soft delete)
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Domain events for event sourcing
        /// </summary>
        private List<INotification> _domainEvents = new List<INotification>();

        public IReadOnlyList<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification eventToAdd)
        {
            _domainEvents.Add(eventToAdd);
        }

        public void RemoveDomainEvent(INotification eventToRemove)
        {
            _domainEvents.Remove(eventToRemove);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
