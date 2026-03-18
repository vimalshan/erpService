namespace CanteenUnit.Application.DTOs;

public record CanteenUnitMasterDto(
    decimal UnComCod,
    string? UnUntName,
    string? UntUntRef,
    decimal? UnMaxVal,
    decimal? InMinVal,
    long? UnSitId,
    long? UnHrmsId);
