using MediatR;
using AutoMapper;
using LocationService.Application.Commands.Locations;
using LocationService.Application.DTOs;
using LocationService.Domain.Aggregates;
using LocationService.Domain.Entities;
using LocationService.Domain.Exceptions;
using LocationService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace LocationService.Application.Handlers.Locations
{
    /// <summary>
    /// Handler for CreateLocationCommand
    /// </summary>
    public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, LocationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateLocationCommandHandler> _logger;

        public CreateLocationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateLocationCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LocationDto> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating location with code: {LocationCode}", request.LocationCode);

            // Check if location already exists
            var existingLocation = await _unitOfWork.Locations.GetByCodeAsync(request.LocationCode, cancellationToken);
            if (existingLocation != null)
                throw new EntityAlreadyExistsException(nameof(LocationAggregate), request.LocationCode);

            // Create new location
            var location = new LocationAggregate(
                locationCode: request.LocationCode,
                locationName: request.LocationName,
                createdBy: request.UserId,
                streetAddress: request.StreetAddress,
                city: request.City,
                state: request.State,
                postalCode: request.PostalCode,
                country: request.Country,
                phone: request.Phone,
                email: request.Email,
                contactPerson: request.ContactPerson);

            await _unitOfWork.Locations.AddAsync(location, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Location created successfully with ID: {LocationId}", location.Id);

            return _mapper.Map<LocationDto>(location);
        }
    }

    /// <summary>
    /// Handler for UpdateLocationCommand
    /// </summary>
    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, LocationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateLocationCommandHandler> _logger;

        public UpdateLocationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateLocationCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LocationDto> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating location with ID: {LocationId}", request.LocationId);

            var location = await _unitOfWork.Locations.GetByIdAsync(request.LocationId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(LocationAggregate), request.LocationId);

            location.UpdateLocationDetails(
                locationName: request.LocationName,
                updatedBy: request.UserId,
                streetAddress: request.StreetAddress,
                city: request.City,
                state: request.State,
                postalCode: request.PostalCode,
                country: request.Country,
                phone: request.Phone,
                email: request.Email,
                contactPerson: request.ContactPerson);

            await _unitOfWork.Locations.UpdateAsync(location, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Location updated successfully");

            return _mapper.Map<LocationDto>(location);
        }
    }

    /// <summary>
    /// Handler for ChangeLocationStatusCommand
    /// </summary>
    public class ChangeLocationStatusCommandHandler : IRequestHandler<ChangeLocationStatusCommand, LocationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ChangeLocationStatusCommandHandler> _logger;

        public ChangeLocationStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ChangeLocationStatusCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LocationDto> Handle(ChangeLocationStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Changing location status. ID: {LocationId}, Status: {Status}", request.LocationId, request.NewStatus);

            var location = await _unitOfWork.Locations.GetByIdAsync(request.LocationId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(LocationAggregate), request.LocationId);

            var newStatus = new Status(request.NewStatus);
            location.ChangeStatus(newStatus, request.UserId);

            await _unitOfWork.Locations.UpdateAsync(location, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Location status changed successfully");

            return _mapper.Map<LocationDto>(location);
        }
    }

    /// <summary>
    /// Handler for DeleteLocationCommand
    /// </summary>
    public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteLocationCommandHandler> _logger;

        public DeleteLocationCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteLocationCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting location with ID: {LocationId}", request.LocationId);

            var location = await _unitOfWork.Locations.GetByIdAsync(request.LocationId, cancellationToken);
            if (location == null)
                throw new EntityNotFoundException(nameof(LocationAggregate), request.LocationId);

            await _unitOfWork.Locations.DeleteAsync(request.LocationId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Location deleted successfully");

            return true;
        }
    }
}
