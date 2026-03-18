using AlertsNotifications.Application.DTOs;
using MediatR;

namespace AlertsNotifications.Application.Features.CircularTemplates.Queries;

public record GetAllCircularTemplatesQuery : IRequest<IEnumerable<CircularTemplateDto>>;
public record GetCircularTemplateByIdQuery(long TemplateId) : IRequest<CircularTemplateDto?>;
public record GetCircularTemplatesByTypeQuery(long TypeId) : IRequest<IEnumerable<CircularTemplateDto>>;
