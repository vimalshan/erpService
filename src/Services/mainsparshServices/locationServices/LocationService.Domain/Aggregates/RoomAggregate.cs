using LocationService.Domain.Entities;
using LocationService.Domain.ValueObjects;
using System.Collections.Generic;

namespace LocationService.Domain.Aggregates
{
    /// <summary>
    /// Room Aggregate Root - Represents a room/meeting space at a location
    /// </summary>
    public class RoomAggregate : Entity
    {
        private readonly List<RoomResourceAggregate> _resources = new();

        public long LocationId { get; private set; }
        public string RoomCode { get; private set; } = string.Empty;
        public string RoomName { get; private set; } = string.Empty;
        public int? RoomCapacity { get; private set; }
        public string? RoomType { get; private set; }
        public int? FloorNumber { get; private set; }
        public Status RoomStatus { get; private set; } = Status.Active;

        public IReadOnlyCollection<RoomResourceAggregate> Resources => _resources.AsReadOnly();

        // EF required constructor
        private RoomAggregate() { }

        public RoomAggregate(
            long locationId,
            string roomCode,
            string roomName,
            long createdBy,
            int? roomCapacity = null,
            string? roomType = null,
            int? floorNumber = null)
        {
            LocationId = locationId;
            RoomCode = roomCode;
            RoomName = roomName;
            RoomCapacity = roomCapacity;
            RoomType = roomType;
            FloorNumber = floorNumber;
            CreatedBy = createdBy;
            RoomStatus = Status.Active;

            RaiseDomainEvent(new RoomCreatedDomainEvent(Id, LocationId, RoomCode, RoomName));
        }

        /// <summary>
        /// Update room details
        /// </summary>
        public void UpdateRoomDetails(
            string roomName,
            long updatedBy,
            int? roomCapacity = null,
            string? roomType = null,
            int? floorNumber = null)
        {
            RoomName = roomName;
            RoomCapacity = roomCapacity;
            RoomType = roomType;
            FloorNumber = floorNumber;
            UpdatedBy = updatedBy;
            UpdatedOn = System.DateTime.UtcNow;

            RaiseDomainEvent(new RoomUpdatedDomainEvent(Id, LocationId, RoomCode, RoomName));
        }

        /// <summary>
        /// Change room status
        /// </summary>
        public void ChangeStatus(Status newStatus, long updatedBy)
        {
            if (RoomStatus?.Equals(newStatus) == true)
                throw new InvalidOperationException("Room is already in this status");

            RoomStatus = newStatus;
            UpdatedBy = updatedBy;
            UpdatedOn = System.DateTime.UtcNow;

            if (newStatus.IsActive)
                RaiseDomainEvent(new RoomActivatedDomainEvent(Id, LocationId, RoomCode));
            else
                RaiseDomainEvent(new RoomDeactivatedDomainEvent(Id, LocationId, RoomCode));
        }

        /// <summary>
        /// Add a resource to the room
        /// </summary>
        public void AddResource(RoomResourceAggregate resource)
        {
            if (_resources.Any(r => r.ResourceCode == resource.ResourceCode))
                throw new InvalidOperationException($"Resource with code '{resource.ResourceCode}' already exists in this room");

            _resources.Add(resource);
        }

        /// <summary>
        /// Remove a resource from the room
        /// </summary>
        public void RemoveResource(long resourceId)
        {
            var resource = _resources.FirstOrDefault(r => r.Id == resourceId);
            if (resource != null)
                _resources.Remove(resource);
        }
    }

    // Domain Events
    public class RoomCreatedDomainEvent : DomainEvent
    {
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }

        public RoomCreatedDomainEvent(long roomId, long locationId, string roomCode, string roomName)
        {
            RoomId = roomId;
            LocationId = locationId;
            RoomCode = roomCode;
            RoomName = roomName;
        }
    }

    public class RoomUpdatedDomainEvent : DomainEvent
    {
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }

        public RoomUpdatedDomainEvent(long roomId, long locationId, string roomCode, string roomName)
        {
            RoomId = roomId;
            LocationId = locationId;
            RoomCode = roomCode;
            RoomName = roomName;
        }
    }

    public class RoomActivatedDomainEvent : DomainEvent
    {
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string RoomCode { get; set; }

        public RoomActivatedDomainEvent(long roomId, long locationId, string roomCode)
        {
            RoomId = roomId;
            LocationId = locationId;
            RoomCode = roomCode;
        }
    }

    public class RoomDeactivatedDomainEvent : DomainEvent
    {
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string RoomCode { get; set; }

        public RoomDeactivatedDomainEvent(long roomId, long locationId, string roomCode)
        {
            RoomId = roomId;
            LocationId = locationId;
            RoomCode = roomCode;
        }
    }
}
