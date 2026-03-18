namespace CalendarService.Application.DTOs;

public record CalendarDto(int Id, string Name, int UnitId, DateTime EffDate, DateTime? ClsDate, string Status);
public record HolidayDto(int Id, DateTime Date, string Description, string Type, int? UnitId);
public record ShiftDto(int Id, string Code, string Name, string InTime, string OutTime, decimal Duration);
public record PatternDto(int Id, string Name, string? Description, int CycleId);
public record PatternDetailDto(int Id, int PatternId, int DayNo, int ShiftId, string? ShiftName);

public record CreateCalendarRequest(string Name, int UnitId, DateTime EffDate, long CreatedBy);
public record UpdateCalendarRequest(string Name, int UnitId, long ModifiedBy);
public record CloseCalendarRequest(DateTime CloseDate, long ModifiedBy);

public record CreateHolidayRequest(DateTime Date, string Description, string Type, long CreatedBy, int? UnitId = null);
public record UpdateHolidayRequest(string Description, string Type, long ModifiedBy);

public record CreateShiftRequest(string Code, string Name, string InTime, string OutTime, long CreatedBy);
public record UpdateShiftRequest(string Name, string InTime, string OutTime, long ModifiedBy);

public record CreatePatternRequest(string Name, int CycleId, long CreatedBy, string? Description = null);
public record UpdatePatternRequest(string Name, string? Description, long ModifiedBy);
public record AddPatternDetailRequest(int DayNo, int ShiftId, long CreatedBy);
