using MediatR;
using TravelRequestService.Application.DTOs;

namespace TravelRequestService.Application.Commands;

public record AddTravelAdvanceCommand : IRequest<TravelAdvanceDto>
{
    public long RequestNumber { get; init; }
    public string CompanyCode { get; init; } = "001";
    public decimal AdvanceAmount { get; init; }
    public long? UnitCode { get; init; }
    public long? EmployeeNumber { get; init; }
}
