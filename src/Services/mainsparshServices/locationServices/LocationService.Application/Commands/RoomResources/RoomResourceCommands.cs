using MediatR;
using LocationService.Application.DTOs;

namespace LocationService.Application.Commands.RoomResources
{
    /// <summary>
    /// Command to create a new room resource
    /// </summary>
    public class CreateRoomResourceCommand : IRequest<RoomResourceDto>
    {
        public long RoomId { get; set; }
        public long LocationId { get; set; }
        public string ResourceCode { get; set; } = string.Empty;
        public string ResourceName { get; set; } = string.Empty;
        public string? ResourceType { get; set; }
        public int? ResourceQuantity { get; set; }
        public long UserId { get; set; }
    }

    /// <summary>
    /// Command to update a room resource
    /// </summary>
    public class UpdateRoomResourceCommand : IRequest<RoomResourceDto>
    {
        public long ResourceId { get; set; }
        public string ResourceName { get; set; } = string.Empty;
        public string? ResourceType { get; set; }
        public int? ResourceQuantity { get; set; }
        public long UserId { get; set; }
    }

    /// <summary>
    /// Command to change resource status
    /// </summary>
    public class ChangeRoomResourceStatusCommand : IRequest<RoomResourceDto>
    {
        public long ResourceId { get; set; }
        public string NewStatus { get; set; } = string.Empty;
        public long UserId { get; set; }
    }

    /// <summary>
    /// Command to delete a room resource
    /// </summary>
    public class DeleteRoomResourceCommand : IRequest<bool>
    {
        public long ResourceId { get; set; }
        public long UserId { get; set; }
    }
}
