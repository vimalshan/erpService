using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Entities;
using EnergyService.Domain.Events;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.Processes.Commands.CreateProcess;

public class CreateProcessCommandHandler : IRequestHandler<CreateProcessCommand, EcProcessDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CreateProcessCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EcProcessDto> Handle(CreateProcessCommand request, CancellationToken ct)
    {
        var entity = new EcProcess
        {
            EcProcessId = request.EcProcessId,
            EcProcessDesc = request.EcProcessDesc,
            EcUnitCode = request.EcUnitCode,
            EcCloseFlag = request.EcCloseFlag,
            LastModifiedBy = request.ModifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };

        entity.AddDomainEvent(new ProcessCreatedEvent(entity.EcProcessId, entity.EcProcessDesc, entity.EcUnitCode));

        await _uow.Processes.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<EcProcessDto>(entity);
    }
}
