namespace MasterDataService.Application.DTOs;

public record RateMasterDto(
    string TrustCode,
    int RateId,
    string? RateTypeCode,
    string? RateEffectiveDate,
    string? RateClosingDate,
    decimal? RateValue,
    string? RateDeleteFlag,
    string? ReworkStatus);

public record CreateRateMasterDto(
    string TrustCode,
    string? RateTypeCode,
    string? RateEffectiveDate,
    decimal? RateValue);

public record UpdateRateMasterDto(
    string TrustCode,
    int RateId,
    decimal? RateValue,
    string? RateClosingDate);
