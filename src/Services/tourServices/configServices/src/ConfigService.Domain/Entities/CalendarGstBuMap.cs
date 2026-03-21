using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class CalendarGstBuMap : AggregateRoot<int>
{
    public string CalendarName { get; private set; } = string.Empty;
    public string? R12Bu { get; private set; }

    private CalendarGstBuMap() { }

    public static CalendarGstBuMap Create(int id, string name, string? r12Bu)
    {
        return new CalendarGstBuMap { Id = id, CalendarName = name, R12Bu = r12Bu };
    }
}
