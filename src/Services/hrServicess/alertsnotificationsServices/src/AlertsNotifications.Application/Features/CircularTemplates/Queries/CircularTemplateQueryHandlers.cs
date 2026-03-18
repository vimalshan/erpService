using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace AlertsNotifications.Application.Features.CircularTemplates.Queries;

public class CircularTemplateQueryHandlers :
    IRequestHandler<GetAllCircularTemplatesQuery, IEnumerable<CircularTemplateDto>>,
    IRequestHandler<GetCircularTemplateByIdQuery, CircularTemplateDto?>,
    IRequestHandler<GetCircularTemplatesByTypeQuery, IEnumerable<CircularTemplateDto>>
{
    private readonly ICircularTemplateRepository _repository;
    private readonly IMapper _mapper;

    public CircularTemplateQueryHandlers(ICircularTemplateRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CircularTemplateDto>> Handle(GetAllCircularTemplatesQuery request, CancellationToken cancellationToken)
    {
        var templates = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CircularTemplateDto>>(templates);
    }

    public async Task<CircularTemplateDto?> Handle(GetCircularTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetByIdAsync(request.TemplateId, cancellationToken);
        return template is null ? null : _mapper.Map<CircularTemplateDto>(template);
    }

    public async Task<IEnumerable<CircularTemplateDto>> Handle(GetCircularTemplatesByTypeQuery request, CancellationToken cancellationToken)
    {
        var templates = await _repository.GetByTypeIdAsync(request.TypeId, cancellationToken);
        return _mapper.Map<IEnumerable<CircularTemplateDto>>(templates);
    }
}
