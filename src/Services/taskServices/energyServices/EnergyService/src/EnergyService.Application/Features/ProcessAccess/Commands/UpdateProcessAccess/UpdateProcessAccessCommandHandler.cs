using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Entities;
using EnergyService.Domain.Events;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.ProcessAccess.Commands.UpdateProcessAccess;

public class UpdateProcessAccessCommandHandler : IRequestHandler<UpdateProcessAccessCommand, EcProcessAccessDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateProcessAccessCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EcProcessAccessDto> Handle(UpdateProcessAccessCommand request, CancellationToken ct)
    {
        var entity = new EcProcessAccess
        {
            PaProcessId = request.ProcessId,
            PaEmpSysId = request.EmployeeSysId,
            PaStartDate = request.StartDate,
            PaCloseDate = request.CloseDate,
            PaLastModifiedBy = request.ModifiedBy,
            PaLastModifiedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")
        };

        entity.AddDomainEvent(new ProcessAccessChangedEvent(
            request.ProcessId, request.EmployeeSysId, request.StartDate, request.CloseDate));

        await _uow.ProcessAccesses.UpsertAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<EcProcessAccessDto>(entity);
    }
}
