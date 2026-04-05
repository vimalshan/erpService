using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using LocationService.Application.Commands.RoomResources;
using LocationService.Application.Queries.RoomResources;
using LocationService.Application.DTOs;
using LocationService.Domain.Aggregates;
using LocationService.Domain.Entities;
using LocationService.Domain.Exceptions;
using LocationService.Domain.ValueObjects;

namespace LocationService.Application.Handlers.RoomResources
{
    public class CreateRoomResourceCommandHandler : IRequestHandler<CreateRoomResourceCommand, RoomResourceDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRoomResourceCommandHandler> _logger;

        public CreateRoomResourceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateRoomResourceCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomResourceDto> Handle(CreateRoomResourceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating room resource with code: {ResourceCode}", request.ResourceCode);

            var resource = new RoomResourceAggregate(
                roomId: request.RoomId,
                locationId: request.LocationId,
                resourceCode: request.ResourceCode,
                resourceName: request.ResourceName,
                createdBy: request.UserId,
                resourceType: request.ResourceType,
                resourceQuantity: request.ResourceQuantity);

            await _unitOfWork.RoomResources.AddAsync(resource, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Room resource created with ID: {ResourceId}", resource.Id);

            return _mapper.Map<RoomResourceDto>(resource);
        }
    }

    public class UpdateRoomResourceCommandHandler : IRequestHandler<UpdateRoomResourceCommand, RoomResourceDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateRoomResourceCommandHandler> _logger;

        public UpdateRoomResourceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateRoomResourceCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomResourceDto> Handle(UpdateRoomResourceCommand request, CancellationToken cancellationToken)
        {
            var resource = await _unitOfWork.RoomResources.GetByIdAsync(request.ResourceId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(RoomResourceAggregate), request.ResourceId);

            resource.UpdateResourceDetails(
                resourceName: request.ResourceName,
                updatedBy: request.UserId,
                resourceType: request.ResourceType,
                resourceQuantity: request.ResourceQuantity);

            await _unitOfWork.RoomResources.UpdateAsync(resource, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoomResourceDto>(resource);
        }
    }

    public class DeleteRoomResourceCommandHandler : IRequestHandler<DeleteRoomResourceCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteRoomResourceCommandHandler> _logger;

        public DeleteRoomResourceCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteRoomResourceCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteRoomResourceCommand request, CancellationToken cancellationToken)
        {
            var resource = await _unitOfWork.RoomResources.GetByIdAsync(request.ResourceId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(RoomResourceAggregate), request.ResourceId);

            await _unitOfWork.RoomResources.DeleteAsync(request.ResourceId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

    public class GetRoomResourceByIdQueryHandler : IRequestHandler<GetRoomResourceByIdQuery, RoomResourceDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomResourceByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomResourceDto?> Handle(GetRoomResourceByIdQuery request, CancellationToken cancellationToken)
        {
            var resource = await _unitOfWork.RoomResources.GetByIdAsync(request.ResourceId, cancellationToken);
            return resource != null ? _mapper.Map<RoomResourceDto>(resource) : null;
        }
    }

    public class GetRoomResourcesByRoomQueryHandler : IRequestHandler<GetRoomResourcesByRoomQuery, IReadOnlyList<RoomResourceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomResourcesByRoomQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<RoomResourceDto>> Handle(GetRoomResourcesByRoomQuery request, CancellationToken cancellationToken)
        {
            var resources = await _unitOfWork.RoomResources.GetByRoomIdAsync(request.RoomId, cancellationToken);
            return _mapper.Map<IReadOnlyList<RoomResourceDto>>(resources);
        }
    }

    public class GetRoomResourcesByLocationQueryHandler : IRequestHandler<GetRoomResourcesByLocationQuery, IReadOnlyList<RoomResourceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomResourcesByLocationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<RoomResourceDto>> Handle(GetRoomResourcesByLocationQuery request, CancellationToken cancellationToken)
        {
            var resources = await _unitOfWork.RoomResources.GetByLocationIdAsync(request.LocationId, cancellationToken);
            return _mapper.Map<IReadOnlyList<RoomResourceDto>>(resources);
        }
    }

    public class GetRoomResourcesByTypeQueryHandler : IRequestHandler<GetRoomResourcesByTypeQuery, IReadOnlyList<RoomResourceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomResourcesByTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<RoomResourceDto>> Handle(GetRoomResourcesByTypeQuery request, CancellationToken cancellationToken)
        {
            var resources = await _unitOfWork.RoomResources.GetByResourceTypeAsync(request.ResourceType, cancellationToken);
            return _mapper.Map<IReadOnlyList<RoomResourceDto>>(resources);
        }
    }
}
