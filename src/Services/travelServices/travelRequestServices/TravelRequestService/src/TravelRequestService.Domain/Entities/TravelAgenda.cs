using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.Entities;

public class TravelAgenda : BaseEntity
{
    public long RequestNumber { get; private set; }
    public int SerialNumber { get; private set; }
    public DateTime? MeetingDate { get; private set; }
    public string? PeopleToMeet { get; private set; }
    public string? DesiredOutcome { get; private set; }
    public string? CityName { get; private set; }

    private TravelAgenda() { }

    public static TravelAgenda Create(
        long requestNumber,
        int serialNumber,
        DateTime? meetingDate,
        string? peopleToMeet,
        string? desiredOutcome,
        string? cityName)
    {
        return new TravelAgenda
        {
            RequestNumber = requestNumber,
            SerialNumber = serialNumber,
            MeetingDate = meetingDate,
            PeopleToMeet = peopleToMeet,
            DesiredOutcome = desiredOutcome,
            CityName = cityName
        };
    }
}
