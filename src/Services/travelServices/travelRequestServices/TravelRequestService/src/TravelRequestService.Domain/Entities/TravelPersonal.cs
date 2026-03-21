using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.Entities;

public class TravelPersonal : BaseEntity
{
    public decimal SerialNumber { get; private set; }
    public decimal RequestNumber { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Reason { get; private set; }
    public decimal? Hours { get; private set; }

    private TravelPersonal() { }

    public static TravelPersonal Create(
        decimal serialNumber,
        decimal requestNumber,
        DateTime? startDate,
        DateTime? endDate,
        string? reason,
        decimal? hours)
    {
        return new TravelPersonal
        {
            SerialNumber = serialNumber,
            RequestNumber = requestNumber,
            StartDate = startDate,
            EndDate = endDate,
            Reason = reason,
            Hours = hours
        };
    }
}
