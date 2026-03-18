using MediatR;

namespace GroupIncentiveService.Application.Commands.UpdateGroupMaster;

public record UpdateGroupMasterCommand(
    int GroupId,
    string GroupName,
    string? GroupDescription,
    long ModifiedBy) : IRequest<Unit>;
