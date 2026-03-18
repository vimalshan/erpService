namespace CalendarService.Infrastructure.Messaging;

public record CalendarCreatedMessage(int CalendarId, string CalendarName, DateTime OccurredOn);
public record HolidayCreatedMessage(int HolidayId, DateTime HolidayDate, string Description, DateTime OccurredOn);
public record ShiftCreatedMessage(int ShiftId, string ShiftCode, string ShiftName, DateTime OccurredOn);
