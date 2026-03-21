using MediatR;

namespace TravelRequestService.Application.Commands;

public record RejectTravelRequestCommand : IRequest<bool>
{
    public long PlanNumber { get; init; }
    public string CompanyCode { get; init; } = "001";
    public long RejectedBy { get; init; }
    public string? Remarks { get; init; }
}
