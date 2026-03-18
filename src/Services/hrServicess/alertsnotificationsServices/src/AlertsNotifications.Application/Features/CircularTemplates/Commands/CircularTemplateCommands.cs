using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Domain.Entities;
using AlertsNotifications.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AlertsNotifications.Application.Features.CircularTemplates.Commands;

public record CreateCircularTemplateCommand(
    long CircularTemplateId,
    long CircularTemplateApplyToUnit,
    long CircularTemplateUnitId,
    long CircularTemplateTypeId,
    string CircularTemplateName,
    string CircularTemplateHtml,
    DateTime? CircularTemplateClsDate,
    long CircularTemplateModifiedBy
) : IRequest<CircularTemplateDto>;

public record UpdateCircularTemplateCommand(
    long CircularTemplateId,
    long CircularTemplateApplyToUnit,
    long CircularTemplateUnitId,
    long CircularTemplateTypeId,
    string CircularTemplateName,
    string CircularTemplateHtml,
    DateTime? CircularTemplateClsDate,
    long CircularTemplateModifiedBy
) : IRequest<Unit>;

public record DeleteCircularTemplateCommand(long CircularTemplateId) : IRequest<Unit>;

public class CircularTemplateCommandHandlers :
    IRequestHandler<CreateCircularTemplateCommand, CircularTemplateDto>,
    IRequestHandler<UpdateCircularTemplateCommand, Unit>,
    IRequestHandler<DeleteCircularTemplateCommand, Unit>
{
    private readonly ICircularTemplateRepository _repository;
    private readonly IMapper _mapper;

    public CircularTemplateCommandHandlers(ICircularTemplateRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CircularTemplateDto> Handle(CreateCircularTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = new CircularTemplate
        {
            CircularTemplateId = request.CircularTemplateId,
            CircularTemplateApplyToUnit = request.CircularTemplateApplyToUnit,
            CircularTemplateUnitId = request.CircularTemplateUnitId,
            CircularTemplateTypeId = request.CircularTemplateTypeId,
            CircularTemplateName = request.CircularTemplateName,
            CircularTemplateHtml = request.CircularTemplateHtml,
            CircularTemplateClsDate = request.CircularTemplateClsDate,
            CircularTemplateModifiedBy = request.CircularTemplateModifiedBy,
            CircularTemplateModifiedOn = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(entity, cancellationToken);
        return _mapper.Map<CircularTemplateDto>(created);
    }

    public async Task<Unit> Handle(UpdateCircularTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.CircularTemplateId, cancellationToken)
            ?? throw new KeyNotFoundException($"Template with ID {request.CircularTemplateId} not found.");

        entity.CircularTemplateApplyToUnit = request.CircularTemplateApplyToUnit;
        entity.CircularTemplateUnitId = request.CircularTemplateUnitId;
        entity.CircularTemplateTypeId = request.CircularTemplateTypeId;
        entity.CircularTemplateName = request.CircularTemplateName;
        entity.CircularTemplateHtml = request.CircularTemplateHtml;
        entity.CircularTemplateClsDate = request.CircularTemplateClsDate;
        entity.CircularTemplateModifiedBy = request.CircularTemplateModifiedBy;
        entity.CircularTemplateModifiedOn = DateTime.UtcNow;

        await _repository.UpdateAsync(entity, cancellationToken);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteCircularTemplateCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.CircularTemplateId, cancellationToken);
        return Unit.Value;
    }
}
