using MediatR;
using GroupIncentiveService.Application.DTOs;

namespace GroupIncentiveService.Application.Commands.CreateGroupIncentive;

public record CreateGroupIncentiveDetailInput(
    long EmployeeId,
    decimal AllocPercentage,
    decimal AllocAmount);

public record CreateGroupIncentiveCommand(
    int GroupId,
    int Month,
    int Year,
    decimal TotalAmount,
    long CreatedBy,
    IReadOnlyList<CreateGroupIncentiveDetailInput> Details) : IRequest<long>;
