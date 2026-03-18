using MediatR;
using LocationService.Application.DTOs;

namespace LocationService.Application.Commands.Rooms
{
    /// <summary>
    /// Command to create a new room
    /// </summary>
    public class CreateRoomCommand : IRequest<RoomDto>
    {
        public long LocationId { get; set; }
        public string RoomCode { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public int? RoomCapacity { get; set; }
        public string? RoomType { get; set; }
        public int? FloorNumber { get; set; }
        public long UserId { get; set; }
    }

    /// <summary>
    /// Command to update a room
    /// </summary>
    public class UpdateRoomCommand : IRequest<RoomDto>
    {
        public long RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int? RoomCapacity { get; set; }
        public string? RoomType { get; set; }
        public int? FloorNumber { get; set; }
        public long UserId { get; set; }
    }

    /// <summary>
    /// Command to change room status
    /// </summary>
    public class ChangeRoomStatusCommand : IRequest<RoomDto>
    {
        public long RoomId { get; set; }
        public string NewStatus { get; set; } = string.Empty;
        public long UserId { get; set; }
    }

    /// <summary>
    /// Command to delete a room
    /// </summary>
    public class DeleteRoomCommand : IRequest<bool>
    {
        public long RoomId { get; set; }
        public long UserId { get; set; }
    }
}
