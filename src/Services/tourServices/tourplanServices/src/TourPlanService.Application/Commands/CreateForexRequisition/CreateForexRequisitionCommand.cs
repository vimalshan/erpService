using MediatR;
using TourPlanService.Application.Common;

namespace TourPlanService.Application.Commands.CreateForexRequisition;

public sealed record CreateForexRequisitionCommand : IRequest<Result<string>>
{
    public string ForReqId { get; init; } = default!;
    public string TpId { get; init; } = default!;
    public string PassNo { get; init; } = default!;
    public string PassName { get; init; } = default!;
    public string PassLocation { get; init; } = default!;
    public DateTime PassExpDate { get; init; }
    public string Type { get; init; } = "R";
    public string AdlRemarks { get; init; } = default!;
    public string AdvRefNo { get; init; } = default!;
    public string CreatedBy { get; init; } = default!;
}
