using AutoMapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Domain.Interfaces;
using MediatR;

namespace MasterDataService.Application.Queries.GuestHouse;

public class GetAllGuestHousesQueryHandler : IRequestHandler<GetAllGuestHousesQuery, IReadOnlyList<GuestHouseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllGuestHousesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<GuestHouseDto>> Handle(GetAllGuestHousesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.GuestHouses.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<GuestHouseDto>>(entities);
    }
}

public class GetGuestHouseByIdQueryHandler : IRequestHandler<GetGuestHouseByIdQuery, GuestHouseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGuestHouseByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GuestHouseDto?> Handle(GetGuestHouseByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.GuestHouses.GetByIdAsync(request.Id, cancellationToken);
        return entity is null ? null : _mapper.Map<GuestHouseDto>(entity);
    }
}

public class GetGuestHouseByAdminCodeQueryHandler : IRequestHandler<GetGuestHouseByAdminCodeQuery, GuestHouseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGuestHouseByAdminCodeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GuestHouseDto?> Handle(GetGuestHouseByAdminCodeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.GuestHouses.GetByAdminCodeAsync(request.AdminCode, cancellationToken);
        return entity is null ? null : _mapper.Map<GuestHouseDto>(entity);
    }
}

public class GetGuestHousesWithRoomsQueryHandler : IRequestHandler<GetGuestHousesWithRoomsQuery, IReadOnlyList<GuestHouseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGuestHousesWithRoomsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<GuestHouseDto>> Handle(GetGuestHousesWithRoomsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.GuestHouses.GetWithRoomsAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<GuestHouseDto>>(entities);
    }
}
