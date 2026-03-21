using AutoMapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Domain.Exceptions;
using MasterDataService.Domain.Interfaces;
using MediatR;

namespace MasterDataService.Application.Commands.GuestHouse;

public class CreateGuestHouseCommandHandler : IRequestHandler<CreateGuestHouseCommand, GuestHouseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateGuestHouseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GuestHouseDto> Handle(CreateGuestHouseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.GuestHouse(request.AdminCode, request.GuestHouseName, request.DailyAmount);
        await _unitOfWork.GuestHouses.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GuestHouseDto>(entity);
    }
}

public class UpdateGuestHouseCommandHandler : IRequestHandler<UpdateGuestHouseCommand, GuestHouseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateGuestHouseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GuestHouseDto> Handle(UpdateGuestHouseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.GuestHouses.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.GuestHouse), request.Id);

        entity.UpdateDetails(request.GuestHouseName, request.DailyAmount);
        await _unitOfWork.GuestHouses.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GuestHouseDto>(entity);
    }
}

public class DeleteGuestHouseCommandHandler : IRequestHandler<DeleteGuestHouseCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGuestHouseCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteGuestHouseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.GuestHouses.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(Domain.Entities.GuestHouse), request.Id);

        await _unitOfWork.GuestHouses.DeleteAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
