using LocationService.Domain.Entities;
using LocationService.Domain.ValueObjects;

namespace LocationService.Domain.Aggregates
{
    /// <summary>
    /// Room Resource Aggregate - Represents equipment/resources in a room
    /// </summary>
    public class RoomResourceAggregate : Entity
    {
        public long RoomId { get; private set; }
        public long LocationId { get; private set; }
        public string ResourceCode { get; private set; } = string.Empty;
        public string ResourceName { get; private set; } = string.Empty;
        public string? ResourceType { get; private set; }
        public int? ResourceQuantity { get; private set; }
        public Status ResourceStatus { get; private set; } = Status.Active;

        // EF required constructor
        private RoomResourceAggregate() { }

        public RoomResourceAggregate(
            long roomId,
            long locationId,
            string resourceCode,
            string resourceName,
            long createdBy,
            string? resourceType = null,
            int? resourceQuantity = null)
        {
            RoomId = roomId;
            LocationId = locationId;
            ResourceCode = resourceCode;
            ResourceName = resourceName;
            ResourceType = resourceType;
            ResourceQuantity = resourceQuantity;
            CreatedBy = createdBy;
            ResourceStatus = Status.Active;

            RaiseDomainEvent(new RoomResourceCreatedDomainEvent(Id, RoomId, LocationId, ResourceCode, ResourceName));
        }

        /// <summary>
        /// Update resource details
        /// </summary>
        public void UpdateResourceDetails(
            string resourceName,
            long updatedBy,
            string? resourceType = null,
            int? resourceQuantity = null)
        {
            ResourceName = resourceName;
            ResourceType = resourceType;
            ResourceQuantity = resourceQuantity;
            UpdatedBy = updatedBy;
            UpdatedOn = System.DateTime.UtcNow;

            RaiseDomainEvent(new RoomResourceUpdatedDomainEvent(Id, RoomId, LocationId, ResourceCode, ResourceName));
        }

        /// <summary>
        /// Change resource status
        /// </summary>
        public void ChangeStatus(Status newStatus, long updatedBy)
        {
            if (ResourceStatus?.Equals(newStatus) == true)
                throw new InvalidOperationException("Resource is already in this status");

            ResourceStatus = newStatus;
            UpdatedBy = updatedBy;
            UpdatedOn = System.DateTime.UtcNow;

            if (newStatus.IsActive)
                RaiseDomainEvent(new RoomResourceActivatedDomainEvent(Id, RoomId, LocationId, ResourceCode));
            else
                RaiseDomainEvent(new RoomResourceDeactivatedDomainEvent(Id, RoomId, LocationId, ResourceCode));
        }

        /// <summary>
        /// Update resource quantity
        /// </summary>
        public void UpdateQuantity(int newQuantity, long updatedBy)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative", nameof(newQuantity));

            ResourceQuantity = newQuantity;
            UpdatedBy = updatedBy;
            UpdatedOn = System.DateTime.UtcNow;

            RaiseDomainEvent(new RoomResourceQuantityUpdatedDomainEvent(Id, RoomId, LocationId, ResourceCode, newQuantity));
        }
    }

    // Domain Events
    public class RoomResourceCreatedDomainEvent : DomainEvent
    {
        public long ResourceId { get; set; }
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string ResourceCode { get; set; }
        public string ResourceName { get; set; }

        public RoomResourceCreatedDomainEvent(long resourceId, long roomId, long locationId, string resourceCode, string resourceName)
        {
            ResourceId = resourceId;
            RoomId = roomId;
            LocationId = locationId;
            ResourceCode = resourceCode;
            ResourceName = resourceName;
        }
    }

    public class RoomResourceUpdatedDomainEvent : DomainEvent
    {
        public long ResourceId { get; set; }
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string ResourceCode { get; set; }
        public string ResourceName { get; set; }

        public RoomResourceUpdatedDomainEvent(long resourceId, long roomId, long locationId, string resourceCode, string resourceName)
        {
            ResourceId = resourceId;
            RoomId = roomId;
            LocationId = locationId;
            ResourceCode = resourceCode;
            ResourceName = resourceName;
        }
    }

    public class RoomResourceActivatedDomainEvent : DomainEvent
    {
        public long ResourceId { get; set; }
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string ResourceCode { get; set; }

        public RoomResourceActivatedDomainEvent(long resourceId, long roomId, long locationId, string resourceCode)
        {
            ResourceId = resourceId;
            RoomId = roomId;
            LocationId = locationId;
            ResourceCode = resourceCode;
        }
    }

    public class RoomResourceDeactivatedDomainEvent : DomainEvent
    {
        public long ResourceId { get; set; }
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string ResourceCode { get; set; }

        public RoomResourceDeactivatedDomainEvent(long resourceId, long roomId, long locationId, string resourceCode)
        {
            ResourceId = resourceId;
            RoomId = roomId;
            LocationId = locationId;
            ResourceCode = resourceCode;
        }
    }

    public class RoomResourceQuantityUpdatedDomainEvent : DomainEvent
    {
        public long ResourceId { get; set; }
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string ResourceCode { get; set; }
        public int NewQuantity { get; set; }

        public RoomResourceQuantityUpdatedDomainEvent(long resourceId, long roomId, long locationId, string resourceCode, int newQuantity)
        {
            ResourceId = resourceId;
            RoomId = roomId;
            LocationId = locationId;
            ResourceCode = resourceCode;
            NewQuantity = newQuantity;
        }
    }
}
