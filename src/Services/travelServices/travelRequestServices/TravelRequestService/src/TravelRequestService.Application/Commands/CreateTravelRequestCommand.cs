using MediatR;
using TravelRequestService.Application.DTOs;

namespace TravelRequestService.Application.Commands;

public record CreateTravelRequestCommand : IRequest<TravelRequestDto>
{
    public string CompanyCode { get; init; } = "001";
    public long UserNumber { get; init; }
    public string? Objective { get; init; }
    public string TravelType { get; init; } = "Domestic";
    public decimal? BudgetAmount { get; init; }
    public List<CreateAgendaItem> Agendas { get; init; } = [];
}

public record CreateAgendaItem
{
    public DateTime? MeetingDate { get; init; }
    public string? PeopleToMeet { get; init; }
    public string? DesiredOutcome { get; init; }
    public string? CityName { get; init; }
}
