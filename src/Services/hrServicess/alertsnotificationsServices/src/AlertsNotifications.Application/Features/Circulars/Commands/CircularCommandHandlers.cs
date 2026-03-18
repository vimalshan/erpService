using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Domain.Events;
using AlertsNotifications.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AlertsNotifications.Application.Features.Circulars.Commands;

public class CircularCommandHandlers :
    IRequestHandler<CreateCircularCommand, CircularDto>,
    IRequestHandler<UpdateCircularCommand, Unit>,
    IRequestHandler<ApproveCircularCommand, Unit>,
    IRequestHandler<DeleteCircularCommand, Unit>
{
    private readonly ICircularRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CircularCommandHandlers(ICircularRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<CircularDto> Handle(CreateCircularCommand request, CancellationToken cancellationToken)
    {
        var entity = new Circular
        {
            CircularId = request.CircularId,
            CircularNo = request.CircularNo,
            CircularYearId = request.CircularYearId,
            CircularType = request.CircularType,
            CircularOrgId = request.CircularOrgId,
            CircularBuSpecific = request.CircularBuSpecific,
            CircularUnitSpecific = request.CircularUnitSpecific,
            CircularHrRoleId = request.CircularHrRoleId,
            CircularVersionNo = request.CircularVersionNo,
            CircularTemplateId = request.CircularTemplateId,
            CircularPdfFileName = request.CircularPdfFileName,
            CircularRtf = request.CircularRtf,
            CircularSignatoryId = request.CircularSignatoryId,
            CircularSparshFlag = request.CircularSparshFlag,
            CircularPostDate = request.CircularPostDate,
            CircularRemoveDate = request.CircularRemoveDate,
            CircularDesc = request.CircularDesc,
            CircularSubject = request.CircularSubject,
            CircularToList = request.CircularToList,
            CircularCcList = request.CircularCcList,
            CircularStatus = request.CircularStatus,
            CircularAttachEmpFlag = request.CircularAttachEmpFlag,
            CreatedBy = request.CreatedBy,
            CreatedOn = DateTime.UtcNow,
            ModifiedBy = request.CreatedBy,
            ModifiedOn = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<CircularDto>(created);
    }

    public async Task<Unit> Handle(UpdateCircularCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.CircularId, cancellationToken)
            ?? throw new KeyNotFoundException($"Circular with ID {request.CircularId} not found.");

        entity.CircularNo = request.CircularNo;
        entity.CircularYearId = request.CircularYearId;
        entity.CircularType = request.CircularType;
        entity.CircularOrgId = request.CircularOrgId;
        entity.CircularBuSpecific = request.CircularBuSpecific;
        entity.CircularUnitSpecific = request.CircularUnitSpecific;
        entity.CircularHrRoleId = request.CircularHrRoleId;
        entity.CircularVersionNo = request.CircularVersionNo;
        entity.CircularTemplateId = request.CircularTemplateId;
        entity.CircularPdfFileName = request.CircularPdfFileName;
        entity.CircularRtf = request.CircularRtf;
        entity.CircularSignatoryId = request.CircularSignatoryId;
        entity.CircularSparshFlag = request.CircularSparshFlag;
        entity.CircularPostDate = request.CircularPostDate;
        entity.CircularRemoveDate = request.CircularRemoveDate;
        entity.CircularDesc = request.CircularDesc;
        entity.CircularSubject = request.CircularSubject;
        entity.CircularToList = request.CircularToList;
        entity.CircularCcList = request.CircularCcList;
        entity.CircularStatus = request.CircularStatus;
        entity.CircularAttachEmpFlag = request.CircularAttachEmpFlag;
        entity.ModifiedBy = request.ModifiedBy;
        entity.ModifiedOn = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);
        return Unit.Value;
    }

    public async Task<Unit> Handle(ApproveCircularCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.CircularId, cancellationToken)
            ?? throw new KeyNotFoundException($"Circular with ID {request.CircularId} not found.");

        var oldStatus = entity.CircularStatus;
        entity.CircularStatus = 'A';
        entity.CircularApprovedBy = request.ApprovedBy;
        entity.CircularApprovedOn = DateTime.UtcNow;
        entity.CircularAppRemarks = request.Remarks;
        entity.ModifiedBy = request.ApprovedBy;
        entity.ModifiedOn = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);
        await _mediator.Publish(new CircularApprovedEvent(entity.CircularId, entity.CircularSubject, request.ApprovedBy), cancellationToken);
        await _mediator.Publish(new CircularStatusChangedEvent(entity.CircularId, oldStatus, 'A'), cancellationToken);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteCircularCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.CircularId, cancellationToken);
        return Unit.Value;
    }
}
