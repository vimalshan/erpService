using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.TourPlan;

public class TourPlanAgenda : Entity<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string PartyToMeet { get; private set; } = string.Empty;
    public string DesiredOutcome { get; private set; } = string.Empty;
    public DateTime? AgendaDate { get; private set; }

    protected TourPlanAgenda() { }

    public static TourPlanAgenda Create(
        string id, string tourPlanId, string city,
        string partyToMeet, string desiredOutcome, DateTime? agendaDate = null)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            City = city,
            PartyToMeet = partyToMeet,
            DesiredOutcome = desiredOutcome,
            AgendaDate = agendaDate
        };
}
