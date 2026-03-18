namespace CardManagement.Application.Common.DTOs;

public record GuestCardDto(
    long CanteenUnit,
    long CardSequence,
    string? CardNumber,
    string? CardName,
    string? ReportingUnit,
    decimal? ReportingDepartment,
    string? CardType,
    DateTime? EffectiveDate,
    DateTime? ClosingDate,
    bool IsActive
);

public record CanteenCardMapDto(
    decimal SysId,
    long CanteenUnit,
    string CardNumber,
    DateTime? EffectiveDate,
    DateTime? ClosingDate,
    DateTime? UpdatedDate
);

public record CardSettlementDto(
    decimal SysId,
    long CanteenUnit,
    string? CardNumber,
    DateTime? SettlementDate,
    DateTime? UpdatedDate
);
