namespace CalendarService.Domain.Exceptions;

public class CalendarNotFoundException(int id)
    : Exception($"Calendar with ID {id} was not found.");

public class ShiftNotFoundException(int id)
    : Exception($"Shift with ID {id} was not found.");

public class HolidayNotFoundException(int id)
    : Exception($"Holiday with ID {id} was not found.");

public class PatternNotFoundException(int id)
    : Exception($"Pattern with ID {id} was not found.");

public class DuplicateCalendarNameException(string name)
    : Exception($"A calendar with name '{name}' already exists.");

public class DuplicateShiftCodeException(string code)
    : Exception($"A shift with code '{code}' already exists.");
