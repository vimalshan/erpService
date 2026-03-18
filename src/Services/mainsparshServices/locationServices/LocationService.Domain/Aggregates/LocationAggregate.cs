using LocationService.Domain.Entities;
using LocationService.Domain.ValueObjects;
using System.Collections.Generic;

namespace LocationService.Domain.Aggregates
{
    /// <summary>
    /// Location Aggregate Root - Represents a physical location/office
    /// </summary>
    public class LocationAggregate : Entity
    {
        private readonly List<RoomAggregate> _rooms = new();

        public string LocationCode { get; private set; } = string.Empty;
        public string LocationName { get; private set; } = string.Empty;
        public Status LocationStatus { get; private set; } = Status.Active;
        public Address Address { get; private set; } = new();
        public Contact Contact { get; private set; } = new();

        public IReadOnlyCollection<RoomAggregate> Rooms => _rooms.AsReadOnly();

        // EF required constructor
        private LocationAggregate() { }

        public LocationAggregate(
            string locationCode,
            string locationName,
            long createdBy,
            string? streetAddress = null,
            string? city = null,
            string? state = null,
            string? postalCode = null,
            string? country = null,
            string? phone = null,
            string? email = null,
            string? contactPerson = null)
        {
            LocationCode = locationCode;
            LocationName = locationName;
            CreatedBy = createdBy;
            LocationStatus = Status.Active;
            Address = new Address(streetAddress, city, state, postalCode, country);
            Contact = new Contact(phone, email, contactPerson);

            RaiseDomainEvent(new LocationCreatedDomainEvent(Id, LocationCode, LocationName));
        }

        /// <summary>
        /// Update location details
        /// </summary>
        public void UpdateLocationDetails(
            string locationName,
            long updatedBy,
            string? streetAddress = null,
            string? city = null,
            string? state = null,
            string? postalCode = null,
            string? country = null,
            string? phone = null,
            string? email = null,
            string? contactPerson = null)
        {
            LocationName = locationName;
            UpdatedBy = updatedBy;
            UpdatedOn = System.DateTime.UtcNow;
            Address = new Address(streetAddress, city, state, postalCode, country);
            Contact = new Contact(phone, email, contactPerson);

            RaiseDomainEvent(new LocationUpdatedDomainEvent(Id, LocationCode, LocationName));
        }

        /// <summary>
        /// Change location status
        /// </summary>
        public void ChangeStatus(Status newStatus, long updatedBy)
        {
            if (LocationStatus?.Equals(newStatus) == true)
                throw new InvalidOperationException("Location is already in this status");

            LocationStatus = newStatus;
            UpdatedBy = updatedBy;
            UpdatedOn = System.DateTime.UtcNow;

            if (newStatus.IsActive)
                RaiseDomainEvent(new LocationActivatedDomainEvent(Id, LocationCode));
            else
                RaiseDomainEvent(new LocationDeactivatedDomainEvent(Id, LocationCode));
        }

        /// <summary>
        /// Add a room to the location
        /// </summary>
        public void AddRoom(RoomAggregate room)
        {
            if (_rooms.Any(r => r.RoomCode == room.RoomCode))
                throw new InvalidOperationException($"Room with code '{room.RoomCode}' already exists in this location");

            _rooms.Add(room);
        }

        /// <summary>
        /// Remove a room from the location
        /// </summary>
        public void RemoveRoom(long roomId)
        {
            var room = _rooms.FirstOrDefault(r => r.Id == roomId);
            if (room != null)
                _rooms.Remove(room);
        }
    }

    // Domain Events
    public class LocationCreatedDomainEvent : DomainEvent
    {
        public long LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }

        public LocationCreatedDomainEvent(long locationId, string locationCode, string locationName)
        {
            LocationId = locationId;
            LocationCode = locationCode;
            LocationName = locationName;
        }
    }

    public class LocationUpdatedDomainEvent : DomainEvent
    {
        public long LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }

        public LocationUpdatedDomainEvent(long locationId, string locationCode, string locationName)
        {
            LocationId = locationId;
            LocationCode = locationCode;
            LocationName = locationName;
        }
    }

    public class LocationActivatedDomainEvent : DomainEvent
    {
        public long LocationId { get; set; }
        public string LocationCode { get; set; }

        public LocationActivatedDomainEvent(long locationId, string locationCode)
        {
            LocationId = locationId;
            LocationCode = locationCode;
        }
    }

    public class LocationDeactivatedDomainEvent : DomainEvent
    {
        public long LocationId { get; set; }
        public string LocationCode { get; set; }

        public LocationDeactivatedDomainEvent(long locationId, string locationCode)
        {
            LocationId = locationId;
            LocationCode = locationCode;
        }
    }
}
