namespace ConfigService.Application.DTOs;

public record TravelCityDto(string CityId, string CountryId, string CityName, string CityCode,
    string LastModifiedBy, DateTime LastModifiedOn);

public record TravelCountryDto(string CountryId, string CountryName, string AirCode,
    string LastModifiedBy, DateTime LastModifiedOn, string? GhAvailable, string? GhRate, string? NmsGhRate);

public record TravelClassDto(string ClassId, string ModeId, string ClassName, string ClassOrder);

public record TravelContactDto(string? ContactId, string? ContactType, string? AdminId,
    string? AdminName, string? EmployeeSysId, string? PhoneNos, string? EmailId);

public record TravelCountryModeMapDto(string MapId, string ModeId, string CountryId);
public record TravelCountrySectorMapDto(string MapId, string CountryId, string ClassId);
public record TravelCountryCurrencyMapDto(string MapId, string CurrencyId, string CountryId);
public record TravelCityModeMapDto(string MapId, string CityId, string ModeId);
public record TravelCitySectorMapDto(string MapId, string CityId, string ClassId, string GradeFCat);
public record TravelBusCitySectorMapDto(string MapId, string CityId, string ClassId, string BusinessId);
public record TravelBuExcludeDto(string ExId, string? EmployeeSysId, string? UnitId);
