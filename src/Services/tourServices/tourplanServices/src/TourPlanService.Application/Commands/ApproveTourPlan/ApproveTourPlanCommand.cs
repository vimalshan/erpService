using MediatR;
using TourPlanService.Application.Common;

namespace TourPlanService.Application.Commands.ApproveTourPlan;

public sealed record ApproveTourPlanCommand(
    string TpId,
    string ApprovedBy,
    string? Remarks = null) : IRequest<Result>;

public sealed record RejectTourPlanCommand(
    string TpId,
    string RejectedBy,
    string Remarks) : IRequest<Result>;
