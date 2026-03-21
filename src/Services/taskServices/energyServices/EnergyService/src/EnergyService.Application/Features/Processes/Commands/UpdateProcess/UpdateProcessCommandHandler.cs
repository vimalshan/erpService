using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.Processes.Commands.UpdateProcess;

public class UpdateProcessCommandHandler : IRequestHandler<UpdateProcessCommand, EcProcessDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateProcessCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EcProcessDto> Handle(UpdateProcessCommand request, CancellationToken ct)
    {
        var entity = await _uow.Processes.GetByIdAsync(request.EcProcessId, ct)
            ?? throw new KeyNotFoundException($"Process {request.EcProcessId} not found.");

        entity.EcProcessDesc = request.EcProcessDesc;
        entity.EcUnitCode = request.EcUnitCode;
        entity.EcCloseFlag = request.EcCloseFlag;
        entity.LastModifiedBy = request.ModifiedBy;
        entity.LastModifiedOn = DateTime.UtcNow;

        _uow.Processes.Update(entity);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<EcProcessDto>(entity);
    }
}
