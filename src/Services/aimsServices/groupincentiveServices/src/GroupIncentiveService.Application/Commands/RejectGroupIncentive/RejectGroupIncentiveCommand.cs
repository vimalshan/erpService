using MediatR;

namespace GroupIncentiveService.Application.Commands.RejectGroupIncentive;

public record RejectGroupIncentiveCommand(
    long IncentiveId,
    string Remarks,
    long RejectedBy) : IRequest<Unit>;
