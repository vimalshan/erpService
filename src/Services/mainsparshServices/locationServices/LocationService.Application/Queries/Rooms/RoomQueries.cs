using MediatR;
using LocationService.Application.DTOs;

namespace LocationService.Application.Queries.Rooms
{
    /// <summary>
    /// Query to get room by ID
    /// </summary>
    public class GetRoomByIdQuery : IRequest<RoomDto?>
    {
        public long RoomId { get; set; }

        public GetRoomByIdQuery(long roomId)
        {
            RoomId = roomId;
        }
    }

    /// <summary>
    /// Query to get room by location and code
    /// </summary>
    public class GetRoomByCodeQuery : IRequest<RoomDto?>
    {
        public long LocationId { get; set; }
        public string RoomCode { get; set; }

        public GetRoomByCodeQuery(long locationId, string roomCode)
        {
            LocationId = locationId;
            RoomCode = roomCode;
        }
    }

    /// <summary>
    /// Query to get all rooms in a location
    /// </summary>
    public class GetRoomsByLocationQuery : IRequest<IReadOnlyList<RoomDto>>
    {
        public long LocationId { get; set; }

        public GetRoomsByLocationQuery(long locationId)
        {
            LocationId = locationId;
        }
    }

    /// <summary>
    /// Query to get rooms by type
    /// </summary>
    public class GetRoomsByTypeQuery : IRequest<IReadOnlyList<RoomDto>>
    {
        public string RoomType { get; set; }

        public GetRoomsByTypeQuery(string roomType)
        {
            RoomType = roomType;
        }
    }

    /// <summary>
    /// Query to get rooms by capacity and location
    /// </summary>
    public class GetRoomsByCapacityQuery : IRequest<IReadOnlyList<RoomDto>>
    {
        public long LocationId { get; set; }
        public int MinCapacity { get; set; }

        public GetRoomsByCapacityQuery(long locationId, int minCapacity)
        {
            LocationId = locationId;
            MinCapacity = minCapacity;
        }
    }
}
