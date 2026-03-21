namespace ConfigService.Application.DTOs;

public record CurrencyDto(long CurrencyId, string CurrencyCode, string? CurrencyName, string? CurrencySymbol);

public record ExpenseCurrencyDto(string CurrencyCode, string CurrencyName, string CurrencyShortName, string CurrencySymbol);

public record ExpenseGroupDto(string GroupId, string GroupName, string TravelType, string BreakFlag, List<ExpenseGroupMapDto>? Mappings);

public record ExpenseGroupMapDto(string MapId, string GroupId, string ExpenseId);

public record ExpenseTypeDto(long ExpenseId, string ExpenseName, int ExpenseCategoryId, string TravelType, long SortNo);

public record GlobalPayParamDto(string ParamId, string ParamCode, string ParamDescription, string ParamValue);

public record CalendarGstBuMapDto(int CalendarId, string CalendarName, string? R12Bu);
