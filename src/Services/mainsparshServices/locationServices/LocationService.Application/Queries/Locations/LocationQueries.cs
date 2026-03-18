using MediatR;
using LocationService.Application.DTOs;

namespace LocationService.Application.Queries.Locations
{
    /// <summary>
    /// Query to get location by ID
    /// </summary>
    public class GetLocationByIdQuery : IRequest<LocationDto?>
    {
        public long LocationId { get; set; }

        public GetLocationByIdQuery(long locationId)
        {
            LocationId = locationId;
        }
    }

    /// <summary>
    /// Query to get location by code
    /// </summary>
    public class GetLocationByCodeQuery : IRequest<LocationDto?>
    {
        public string LocationCode { get; set; }

        public GetLocationByCodeQuery(string locationCode)
        {
            LocationCode = locationCode;
        }
    }

    /// <summary>
    /// Query to get all locations
    /// </summary>
    public class GetAllLocationsQuery : IRequest<IReadOnlyList<LocationDto>>
    {
    }

    /// <summary>
    /// Query to get all active locations
    /// </summary>
    public class GetActiveLocationsQuery : IRequest<IReadOnlyList<LocationDto>>
    {
    }

    /// <summary>
    /// Query to search locations by name
    /// </summary>
    public class SearchLocationsByNameQuery : IRequest<IReadOnlyList<LocationDto>>
    {
        public string SearchText { get; set; }

        public SearchLocationsByNameQuery(string searchText)
        {
            SearchText = searchText;
        }
    }
}
