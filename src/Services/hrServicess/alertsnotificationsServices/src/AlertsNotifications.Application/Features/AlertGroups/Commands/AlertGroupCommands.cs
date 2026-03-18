using AlertsNotifications.Application.DTOs;
using MediatR;

namespace AlertsNotifications.Application.Features.AlertGroups.Commands;

public record CreateAlertGroupCommand(
    decimal AlertGroupId,
    string AlertGroupName,
    char AlertGroupType,
    long CreatedBy
) : IRequest<AlertGroupDto>;

public record UpdateAlertGroupCommand(
    decimal AlertGroupId,
    string AlertGroupName,
    char AlertGroupType,
    long ModifiedBy
) : IRequest<Unit>;

public record DeleteAlertGroupCommand(decimal AlertGroupId) : IRequest<Unit>;
