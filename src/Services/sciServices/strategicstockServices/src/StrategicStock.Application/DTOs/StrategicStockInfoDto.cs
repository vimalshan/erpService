namespace StrategicStock.Application.DTOs;

public sealed record StrategicStockInfoDto(
    int StrategicStockId,
    string? StrategicStockType,
    long? MaxQty,
    long? FilledQty,
    string? EffectiveDate);
