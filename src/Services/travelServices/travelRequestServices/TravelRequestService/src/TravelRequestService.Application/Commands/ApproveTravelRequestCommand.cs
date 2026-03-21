using MediatR;

namespace TravelRequestService.Application.Commands;

public record ApproveTravelRequestCommand : IRequest<bool>
{
    public long PlanNumber { get; init; }
    public string CompanyCode { get; init; } = "001";
    public long ApprovedBy { get; init; }
    public decimal ApprovalAmount { get; init; }
    public string? Remarks { get; init; }
}
