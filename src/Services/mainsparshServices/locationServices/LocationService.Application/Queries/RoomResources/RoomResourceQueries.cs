using MediatR;
using LocationService.Application.DTOs;

namespace LocationService.Application.Queries.RoomResources
{
    /// <summary>
    /// Query to get resource by ID
    /// </summary>
    public class GetRoomResourceByIdQuery : IRequest<RoomResourceDto?>
    {
        public long ResourceId { get; set; }

        public GetRoomResourceByIdQuery(long resourceId)
        {
            ResourceId = resourceId;
        }
    }

    /// <summary>
    /// Query to get resources by room
    /// </summary>
    public class GetRoomResourcesByRoomQuery : IRequest<IReadOnlyList<RoomResourceDto>>
    {
        public long RoomId { get; set; }

        public GetRoomResourcesByRoomQuery(long roomId)
        {
            RoomId = roomId;
        }
    }

    /// <summary>
    /// Query to get resources by location
    /// </summary>
    public class GetRoomResourcesByLocationQuery : IRequest<IReadOnlyList<RoomResourceDto>>
    {
        public long LocationId { get; set; }

        public GetRoomResourcesByLocationQuery(long locationId)
        {
            LocationId = locationId;
        }
    }

    /// <summary>
    /// Query to get resources by type
    /// </summary>
    public class GetRoomResourcesByTypeQuery : IRequest<IReadOnlyList<RoomResourceDto>>
    {
        public string ResourceType { get; set; }

        public GetRoomResourcesByTypeQuery(string resourceType)
        {
            ResourceType = resourceType;
        }
    }

    /// <summary>
    /// Query to search resources by name
    /// </summary>
    public class SearchRoomResourcesQuery : IRequest<IReadOnlyList<RoomResourceDto>>
    {
        public string SearchText { get; set; }

        public SearchRoomResourcesQuery(string searchText)
        {
            SearchText = searchText;
        }
    }
}
