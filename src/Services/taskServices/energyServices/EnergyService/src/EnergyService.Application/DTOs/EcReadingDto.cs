namespace EnergyService.Application.DTOs;

public record EcReadingDto(
    int? EbId,
    string EbUnitCode,
    int EbProcessId,
    DateTime EbDate,
    long? EbTarget,
    long? EbReading,
    long? EbResetReading,
    long? EbActualUsage,
    long? EbToDate,
    string? EbRemarks,
    int LastModifiedBy,
    DateTime LastModifiedOn);
