using AuditService.Application.DTOs;
using AuditService.Domain.Interfaces;
using MediatR;

namespace AuditService.Application.Queries.Audits;

public sealed class GetAuditByIdQueryHandler : IRequestHandler<GetAuditByIdQuery, AuditDto?>
{
    private readonly IAuditRepository _auditRepository;

    public GetAuditByIdQueryHandler(IAuditRepository auditRepository) => _auditRepository = auditRepository;

    public async Task<AuditDto?> Handle(GetAuditByIdQuery request, CancellationToken cancellationToken)
    {
        var audit = await _auditRepository.GetByIdAsync(request.AuditId, cancellationToken);
        if (audit is null) return null;

        return new AuditDto(
            audit.AuditId, audit.AuditName, audit.AuditUnit, audit.AuditFrom, audit.AuditTo,
            audit.AuditDefLocation, audit.AuditStatus, audit.AuditCreatedBy, audit.AuditCreatedOn,
            audit.AuditPlanFrom, audit.AuditPlanTo, audit.AuditCompleted, audit.AuditFirmName,
            audit.AuditProcess, audit.Observations.Count);
    }
}

public sealed class GetAllAuditsQueryHandler : IRequestHandler<GetAllAuditsQuery, IEnumerable<AuditDto>>
{
    private readonly IAuditRepository _auditRepository;

    public GetAllAuditsQueryHandler(IAuditRepository auditRepository) => _auditRepository = auditRepository;

    public async Task<IEnumerable<AuditDto>> Handle(GetAllAuditsQuery request, CancellationToken cancellationToken)
    {
        var audits = await _auditRepository.GetAllAsync(cancellationToken);
        return audits.Select(a => new AuditDto(
            a.AuditId, a.AuditName, a.AuditUnit, a.AuditFrom, a.AuditTo,
            a.AuditDefLocation, a.AuditStatus, a.AuditCreatedBy, a.AuditCreatedOn,
            a.AuditPlanFrom, a.AuditPlanTo, a.AuditCompleted, a.AuditFirmName,
            a.AuditProcess, a.Observations.Count));
    }
}

public sealed class GetAuditsByUnitQueryHandler : IRequestHandler<GetAuditsByUnitQuery, IEnumerable<AuditDto>>
{
    private readonly IAuditRepository _auditRepository;

    public GetAuditsByUnitQueryHandler(IAuditRepository auditRepository) => _auditRepository = auditRepository;

    public async Task<IEnumerable<AuditDto>> Handle(GetAuditsByUnitQuery request, CancellationToken cancellationToken)
    {
        var audits = await _auditRepository.GetByUnitAsync(request.UnitId, cancellationToken);
        return audits.Select(a => new AuditDto(
            a.AuditId, a.AuditName, a.AuditUnit, a.AuditFrom, a.AuditTo,
            a.AuditDefLocation, a.AuditStatus, a.AuditCreatedBy, a.AuditCreatedOn,
            a.AuditPlanFrom, a.AuditPlanTo, a.AuditCompleted, a.AuditFirmName,
            a.AuditProcess, a.Observations.Count));
    }
}
