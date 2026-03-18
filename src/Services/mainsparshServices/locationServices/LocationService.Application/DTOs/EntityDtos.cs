using System;
using LocationService.Domain.ValueObjects;

namespace LocationService.Application.DTOs
{
    /// <summary>
    /// Location DTO for API responses
    /// </summary>
    public class LocationDto
    {
        public long LocationId { get; set; }
        public string LocationCode { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string LocationStatus { get; set; } = string.Empty;
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ContactPerson { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }

    /// <summary>
    /// Create Location Request DTO
    /// </summary>
    public class CreateLocationDto
    {
        public string LocationCode { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ContactPerson { get; set; }
    }

    /// <summary>
    /// Update Location Request DTO
    /// </summary>
    public class UpdateLocationDto
    {
        public long LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ContactPerson { get; set; }
    }

    /// <summary>
    /// Room DTO for API responses
    /// </summary>
    public class RoomDto
    {
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string RoomCode { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public int? RoomCapacity { get; set; }
        public string? RoomType { get; set; }
        public int? FloorNumber { get; set; }
        public string RoomStatus { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }

    /// <summary>
    /// Create Room Request DTO
    /// </summary>
    public class CreateRoomDto
    {
        public long LocationId { get; set; }
        public string RoomCode { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public int? RoomCapacity { get; set; }
        public string? RoomType { get; set; }
        public int? FloorNumber { get; set; }
    }

    /// <summary>
    /// Update Room Request DTO
    /// </summary>
    public class UpdateRoomDto
    {
        public long RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int? RoomCapacity { get; set; }
        public string? RoomType { get; set; }
        public int? FloorNumber { get; set; }
    }

    /// <summary>
    /// Room Resource DTO for API responses
    /// </summary>
    public class RoomResourceDto
    {
        public long ResourceId { get; set; }
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string ResourceCode { get; set; } = string.Empty;
        public string ResourceName { get; set; } = string.Empty;
        public string? ResourceType { get; set; }
        public int? ResourceQuantity { get; set; }
        public string ResourceStatus { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }

    /// <summary>
    /// Create Room Resource Request DTO
    /// </summary>
    public class CreateRoomResourceDto
    {
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string ResourceCode { get; set; } = string.Empty;
        public string ResourceName { get; set; } = string.Empty;
        public string? ResourceType { get; set; }
        public int? ResourceQuantity { get; set; }
    }

    /// <summary>
    /// Update Room Resource Request DTO
    /// </summary>
    public class UpdateRoomResourceDto
    {
        public long ResourceId { get; set; }
        public string ResourceName { get; set; } = string.Empty;
        public string? ResourceType { get; set; }
        public int? ResourceQuantity { get; set; }
    }
}
