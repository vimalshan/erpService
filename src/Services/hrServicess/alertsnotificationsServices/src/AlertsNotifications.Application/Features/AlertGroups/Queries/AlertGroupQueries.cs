using AlertsNotifications.Application.DTOs;
using MediatR;

namespace AlertsNotifications.Application.Features.AlertGroups.Queries;

public record GetAllAlertGroupsQuery : IRequest<IEnumerable<AlertGroupDto>>;

public record GetAlertGroupByIdQuery(decimal AlertGroupId) : IRequest<AlertGroupDto?>;
