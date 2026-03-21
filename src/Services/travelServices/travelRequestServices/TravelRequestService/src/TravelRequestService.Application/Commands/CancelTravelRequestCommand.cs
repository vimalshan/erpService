using MediatR;

namespace TravelRequestService.Application.Commands;

public record CancelTravelRequestCommand : IRequest<bool>
{
    public long PlanNumber { get; init; }
    public string CompanyCode { get; init; } = "001";
    public string? Remarks { get; init; }
}
