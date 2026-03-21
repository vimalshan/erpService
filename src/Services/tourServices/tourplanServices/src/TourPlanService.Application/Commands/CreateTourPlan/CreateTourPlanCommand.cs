using MediatR;
using TourPlanService.Application.Common;

namespace TourPlanService.Application.Commands.CreateTourPlan;

public sealed record CreateTourPlanCommand : IRequest<Result<string>>
{
    public string TpId { get; init; } = default!;
    public string TpEmpSysId { get; init; } = default!;
    public DateTime TpStartDate { get; init; }
    public DateTime? TpEndDate { get; init; }
    public string TpPurpose { get; init; } = default!;
    public string TpRemarks { get; init; } = default!;
    public string TpCategory { get; init; } = default!;
    public string TpBookInc { get; init; } = default!;
    public string? TpType { get; init; }
    public string TpFromCityId { get; init; } = default!;
    public string TpFromCityName { get; init; } = default!;
    public string TpToCityId { get; init; } = default!;
    public string TpToCityName { get; init; } = default!;
    public string TpSupRemarks { get; init; } = default!;
    public string? TpContactNo { get; init; }
    public string? TpGradeType { get; init; }
    public string CreatedBy { get; init; } = default!;
}
