using AutoMapper;
using CanteenUnit.Application.DTOs;
using CanteenUnit.Domain.Entities;
using CanteenUnit.Domain.Interfaces;
using MediatR;

namespace CanteenUnit.Application.Features.CanteenUnits.Commands.CreateCanteenUnit;

public class CreateCanteenUnitCommandHandler : IRequestHandler<CreateCanteenUnitCommand, CanteenUnitMasterDto>
{
    private readonly ICanteenUnitRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCanteenUnitCommandHandler(
        ICanteenUnitRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CanteenUnitMasterDto> Handle(CreateCanteenUnitCommand request, CancellationToken ct)
    {
        if (await _repository.ExistsAsync(request.ComCode, ct))
            throw new InvalidOperationException($"Canteen unit with code {request.ComCode} already exists.");

        var entity = CanteenUnitMaster.Create(
            request.ComCode, request.UnitName, request.UnitRef,
            request.MaxVal, request.MinVal, request.SiteId, request.HrmsId);

        await _repository.AddAsync(entity, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<CanteenUnitMasterDto>(entity);
    }
}
