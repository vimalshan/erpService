using ContributionService.Application.DTOs;
using MediatR;

namespace ContributionService.Application.Commands.ContributionBatch;

public record CreateContributionBatchCommand(
    string TrustCode,
    string Category,
    string PayunitCode,
    DateTime PayMonthStart,
    DateTime PayMonthEnd
) : IRequest<ContributionMainDto>;

public record PostContributionBatchCommand(
    long BatchNo,
    long PostedByUserId
) : IRequest<ContributionMainDto>;

public record UpdateContributionBatchStatusCommand(
    long BatchNo,
    string Status
) : IRequest<ContributionMainDto>;

public record ProcessMonthlyContributionCommand(
    string MonthYear,
    long ProcessedByUserId
) : IRequest<ProcessContributionResultDto>;
