using AuditService.Application.DTOs;
using AuditService.Domain.Entities;
using AuditService.Domain.Interfaces;
using MediatR;

namespace AuditService.Application.Commands.Audits;

public sealed class CreateAuditCommandHandler : IRequestHandler<CreateAuditCommand, AuditDto>
{
    private readonly IAuditRepository _auditRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAuditCommandHandler(IAuditRepository auditRepository, IUnitOfWork unitOfWork)
    {
        _auditRepository = auditRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuditDto> Handle(CreateAuditCommand request, CancellationToken cancellationToken)
    {
        var audit = AuditMaster.Create(
            request.AuditId, request.AuditName, request.AuditUnit,
            request.AuditFrom, request.AuditTo, request.AuditDefLocation,
            request.AuditPlanFrom, request.AuditPlanTo, request.CreatedBy);

        await _auditRepository.AddAsync(audit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(audit);
    }

    private static AuditDto ToDto(AuditMaster a) => new(
        a.AuditId, a.AuditName, a.AuditUnit, a.AuditFrom, a.AuditTo,
        a.AuditDefLocation, a.AuditStatus.ToString(), a.AuditCreatedBy, a.AuditCreatedOn,
        a.AuditPlanFrom, a.AuditPlanTo, a.AuditCompleted?.ToString(), a.AuditFirmName,
        a.AuditProcess, a.Observations.Count);
}
