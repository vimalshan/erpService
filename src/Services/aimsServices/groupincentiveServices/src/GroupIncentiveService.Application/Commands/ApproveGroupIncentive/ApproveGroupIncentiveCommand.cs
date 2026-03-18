using MediatR;

namespace GroupIncentiveService.Application.Commands.ApproveGroupIncentive;

public record ApproveGroupIncentiveCommand(
    long IncentiveId,
    decimal ApprovedAmount,
    long ApprovedBy) : IRequest<Unit>;
