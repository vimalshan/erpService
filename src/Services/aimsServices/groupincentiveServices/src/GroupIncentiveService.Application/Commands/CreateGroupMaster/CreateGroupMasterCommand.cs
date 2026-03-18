using MediatR;

namespace GroupIncentiveService.Application.Commands.CreateGroupMaster;

public record CreateGroupMasterCommand(
    string GroupName,
    string? GroupDescription,
    DateTime GroupEffDate,
    long CreatedBy) : IRequest<int>;
