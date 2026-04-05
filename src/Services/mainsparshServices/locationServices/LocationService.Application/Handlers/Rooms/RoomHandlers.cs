using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using LocationService.Application.Commands.Rooms;
using LocationService.Application.Queries.Rooms;
using LocationService.Application.DTOs;
using LocationService.Domain.Aggregates;
using LocationService.Domain.Entities;
using LocationService.Domain.Exceptions;
using LocationService.Domain.ValueObjects;

namespace LocationService.Application.Handlers.Rooms
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRoomCommandHandler> _logger;

        public CreateRoomCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateRoomCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating room with code: {RoomCode}", request.RoomCode);

            var room = new RoomAggregate(
                locationId: request.LocationId,
                roomCode: request.RoomCode,
                roomName: request.RoomName,
                createdBy: request.UserId,
                roomCapacity: request.RoomCapacity,
                roomType: request.RoomType,
                floorNumber: request.FloorNumber);

            await _unitOfWork.Rooms.AddAsync(room, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Room created successfully with ID: {RoomId}", room.Id);

            return _mapper.Map<RoomDto>(room);
        }
    }

    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, RoomDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateRoomCommandHandler> _logger;

        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateRoomCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(RoomAggregate), request.RoomId);

            room.UpdateRoomDetails(
                roomName: request.RoomName,
                updatedBy: request.UserId,
                roomCapacity: request.RoomCapacity,
                roomType: request.RoomType,
                floorNumber: request.FloorNumber);

            await _unitOfWork.Rooms.UpdateAsync(room, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RoomDto>(room);
        }
    }

    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteRoomCommandHandler> _logger;

        public DeleteRoomCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteRoomCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(RoomAggregate), request.RoomId);

            await _unitOfWork.Rooms.DeleteAsync(request.RoomId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomDto?> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId, cancellationToken);
            return room != null ? _mapper.Map<RoomDto>(room) : null;
        }
    }

    public class GetRoomsByLocationQueryHandler : IRequestHandler<GetRoomsByLocationQuery, IReadOnlyList<RoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomsByLocationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<RoomDto>> Handle(GetRoomsByLocationQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _unitOfWork.Rooms.GetByLocationIdAsync(request.LocationId, cancellationToken);
            return _mapper.Map<IReadOnlyList<RoomDto>>(rooms);
        }
    }

    public class GetRoomsByTypeQueryHandler : IRequestHandler<GetRoomsByTypeQuery, IReadOnlyList<RoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomsByTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<RoomDto>> Handle(GetRoomsByTypeQuery request, CancellationToken cancellationToken)
        {
            // Get all rooms across all locations and filter by type
            var allRooms = new List<LocationService.Domain.Aggregates.RoomAggregate>();
            // Use RoomResources repo indirectly — scan all rooms via a broad location query
            // Simpler: inject DbContext via IUnitOfWork is not available, so use the Location repo
            // to get all locations then their rooms
            var locations = await _unitOfWork.Locations.GetAllAsync(cancellationToken);
            foreach (var loc in locations)
            {
                var rooms = await _unitOfWork.Rooms.GetByLocationIdAsync(loc.Id, cancellationToken);
                allRooms.AddRange(rooms.Where(r => r.RoomType == request.RoomType));
            }
            return _mapper.Map<IReadOnlyList<RoomDto>>(allRooms);
        }
    }

    public class GetRoomsByCapacityQueryHandler : IRequestHandler<GetRoomsByCapacityQuery, IReadOnlyList<RoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomsByCapacityQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<RoomDto>> Handle(GetRoomsByCapacityQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _unitOfWork.Rooms.GetByLocationIdAsync(request.LocationId, cancellationToken);
            var filtered = rooms.Where(r => r.RoomCapacity >= request.MinCapacity).ToList();
            return _mapper.Map<IReadOnlyList<RoomDto>>(filtered);
        }
    }
}
