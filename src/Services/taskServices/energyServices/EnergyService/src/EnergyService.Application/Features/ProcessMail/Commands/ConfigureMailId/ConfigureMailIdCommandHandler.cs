using AutoMapper;
using EnergyService.Application.DTOs;
using EnergyService.Domain.Entities;
using EnergyService.Domain.Interfaces;
using MediatR;

namespace EnergyService.Application.Features.ProcessMail.Commands.ConfigureMailId;

public class ConfigureMailIdCommandHandler : IRequestHandler<ConfigureMailIdCommand, EcProcessMailIdDto>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ConfigureMailIdCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<EcProcessMailIdDto> Handle(ConfigureMailIdCommand request, CancellationToken ct)
    {
        var entity = new EcProcessMailId
        {
            PmProcessId = request.ProcessId,
            PmMailId = request.MailId,
            PmDeliveryType = request.DeliveryType,
            PmStartDate = request.StartDate,
            PmCloseDate = request.CloseDate,
            PmLastModifiedBy = request.ModifiedBy,
            PmLastModifiedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        };

        await _uow.ProcessMailIds.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);

        return _mapper.Map<EcProcessMailIdDto>(entity);
    }
}
