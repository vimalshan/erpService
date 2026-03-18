using MediatR;
using LocationService.Application.DTOs;

namespace LocationService.Application.Commands.Locations
{
    /// <summary>
    /// Command to create a new location
    /// </summary>
    public class CreateLocationCommand : IRequest<LocationDto>
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
        public long UserId { get; set; }
    }

    /// <summary>
    /// Command to update a location
    /// </summary>
    public class UpdateLocationCommand : IRequest<LocationDto>
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
        public long UserId { get; set; }
    }

    /// <summary>
    /// Command to change location status
    /// </summary>
    public class ChangeLocationStatusCommand : IRequest<LocationDto>
    {
        public long LocationId { get; set; }
        public string NewStatus { get; set; } = string.Empty;
        public long UserId { get; set; }
    }

    /// <summary>
    /// Command to delete a location
    /// </summary>
    public class DeleteLocationCommand : IRequest<bool>
    {
        public long LocationId { get; set; }
        public long UserId { get; set; }
    }
}
