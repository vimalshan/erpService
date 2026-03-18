using MediatR;
using AutoMapper;
using LocationService.Application.Queries.Locations;
using LocationService.Application.DTOs;
using LocationService.Domain.Entities;
using LocationService.Domain.Exceptions;

namespace LocationService.Application.Handlers.Locations
{
    /// <summary>
    /// Handlers for Location Queries
    /// </summary>
    public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, LocationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLocationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LocationDto?> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(request.LocationId, cancellationToken);
            return location != null ? _mapper.Map<LocationDto>(location) : null;
        }
    }

    public class GetLocationByCodeQueryHandler : IRequestHandler<GetLocationByCodeQuery, LocationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLocationByCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LocationDto?> Handle(GetLocationByCodeQuery request, CancellationToken cancellationToken)
        {
            var location = await _unitOfWork.Locations.GetByCodeAsync(request.LocationCode, cancellationToken);
            return location != null ? _mapper.Map<LocationDto>(location) : null;
        }
    }

    public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, IReadOnlyList<LocationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllLocationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<LocationDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await _unitOfWork.Locations.GetAllAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<LocationDto>>(locations);
        }
    }

    public class GetActiveLocationsQueryHandler : IRequestHandler<GetActiveLocationsQuery, IReadOnlyList<LocationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetActiveLocationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<LocationDto>> Handle(GetActiveLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await _unitOfWork.Locations.GetActiveAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<LocationDto>>(locations);
        }
    }

    public class SearchLocationsByNameQueryHandler : IRequestHandler<SearchLocationsByNameQuery, IReadOnlyList<LocationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchLocationsByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<LocationDto>> Handle(SearchLocationsByNameQuery request, CancellationToken cancellationToken)
        {
            var allLocations = await _unitOfWork.Locations.GetAllAsync(cancellationToken);
            var filtered = allLocations
                .Where(l => l.LocationName.Contains(request.SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return _mapper.Map<IReadOnlyList<LocationDto>>(filtered);
        }
    }
}
