using ScheduleService.Application.DTOs;
using ScheduleService.Domain.Entities;
using ScheduleService.Domain.Interfaces;
using MediatR;

namespace ScheduleService.Application.Commands;

public class ScheduleAuditCommandHandler : IRequestHandler<ScheduleAuditCommand, AuditSiteAuditDto>
{
    private readonly IScheduleDomainRepository _repo;
    private readonly IMediator _mediator;
    public ScheduleAuditCommandHandler(IScheduleDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<AuditSiteAuditDto> Handle(ScheduleAuditCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var entity = AuditSiteAudit.Schedule(d.AuditId, d.SiteId, d.AuditTypeId, d.AuditNumber,
            d.ScheduledDate, d.LeadAuditorId, d.CreatedBy);
        entity.Notes = d.Notes;
        var created = await _repo.AddAsync(entity);
        foreach (var evt in created.DomainEvents) await _mediator.Publish(evt, ct);
        created.ClearDomainEvents();
        return MapToDto(created);
    }

    internal static AuditSiteAuditDto MapToDto(AuditSiteAudit a) => new(
        a.AuditSiteAuditId, a.AuditId, a.SiteId, a.AuditTypeId, a.AuditNumber,
        a.ScheduledDate, a.StartDate, a.EndDate, a.CompletedDate, a.Status,
        a.LeadAuditorId, a.IsActive, a.CreatedDate, a.ModifiedDate, a.CreatedBy, a.ModifiedBy,
        a.Notes, a.ReportPath, a.CertificateIssued, a.CertificateNumber);
}

public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, AuditSiteAuditDto>
{
    private readonly IScheduleDomainRepository _repo;
    public UpdateScheduleCommandHandler(IScheduleDomainRepository repo) { _repo = repo; }

    public async Task<AuditSiteAuditDto> Handle(UpdateScheduleCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var e = await _repo.GetByIdAsync(d.AuditSiteAuditId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Schedule {d.AuditSiteAuditId} not found");
        e.AuditId = d.AuditId; e.SiteId = d.SiteId; e.AuditTypeId = d.AuditTypeId;
        e.AuditNumber = d.AuditNumber; e.ScheduledDate = d.ScheduledDate;
        e.StartDate = d.StartDate; e.EndDate = d.EndDate; e.Status = d.Status;
        e.LeadAuditorId = d.LeadAuditorId; e.IsActive = d.IsActive; e.Notes = d.Notes;
        e.ReportPath = d.ReportPath; e.CertificateIssued = d.CertificateIssued;
        e.CertificateNumber = d.CertificateNumber; e.ModifiedDate = DateTime.UtcNow; e.ModifiedBy = d.ModifiedBy;
        await _repo.UpdateAsync(e);
        return ScheduleAuditCommandHandler.MapToDto(e);
    }
}

public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, bool>
{
    private readonly IScheduleDomainRepository _repo;
    public DeleteScheduleCommandHandler(IScheduleDomainRepository repo) { _repo = repo; }
    public async Task<bool> Handle(DeleteScheduleCommand request, CancellationToken ct)
    {
        await _repo.DeleteAsync(request.AuditSiteAuditId); return true;
    }
}

public class RescheduleAuditCommandHandler : IRequestHandler<RescheduleAuditCommand, AuditSiteAuditDto>
{
    private readonly IScheduleDomainRepository _repo;
    private readonly IMediator _mediator;
    public RescheduleAuditCommandHandler(IScheduleDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<AuditSiteAuditDto> Handle(RescheduleAuditCommand request, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(request.AuditSiteAuditId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Schedule {request.AuditSiteAuditId} not found");
        e.Reschedule(request.NewDate, request.ModifiedBy);
        await _repo.UpdateAsync(e);
        foreach (var evt in e.DomainEvents) await _mediator.Publish(evt, ct);
        e.ClearDomainEvents();
        return ScheduleAuditCommandHandler.MapToDto(e);
    }
}

public class StartAuditCommandHandler : IRequestHandler<StartAuditCommand, AuditSiteAuditDto>
{
    private readonly IScheduleDomainRepository _repo;
    private readonly IMediator _mediator;
    public StartAuditCommandHandler(IScheduleDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<AuditSiteAuditDto> Handle(StartAuditCommand request, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(request.AuditSiteAuditId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Schedule {request.AuditSiteAuditId} not found");
        e.Start(request.StartDate, request.ModifiedBy);
        await _repo.UpdateAsync(e);
        foreach (var evt in e.DomainEvents) await _mediator.Publish(evt, ct);
        e.ClearDomainEvents();
        return ScheduleAuditCommandHandler.MapToDto(e);
    }
}

public class CompleteAuditCommandHandler : IRequestHandler<CompleteAuditCommand, AuditSiteAuditDto>
{
    private readonly IScheduleDomainRepository _repo;
    private readonly IMediator _mediator;
    public CompleteAuditCommandHandler(IScheduleDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<AuditSiteAuditDto> Handle(CompleteAuditCommand request, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(request.AuditSiteAuditId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Schedule {request.AuditSiteAuditId} not found");
        e.Complete(request.CompletedDate, request.ReportPath, request.ModifiedBy);
        await _repo.UpdateAsync(e);
        foreach (var evt in e.DomainEvents) await _mediator.Publish(evt, ct);
        e.ClearDomainEvents();
        return ScheduleAuditCommandHandler.MapToDto(e);
    }
}
