namespace StipendService.Application.DTOs;

public record StipendMasterDto(
    long StipendId,
    long ResearchCategoryId,
    long SrfRankId,
    decimal SrfMonthlyStipend,
    decimal? AdditionalAllowance,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo,
    string Status,
    long CreatedBy,
    DateTime CreatedOn,
    long? UpdatedBy,
    DateTime? UpdatedOn
);

public record StipendDisbursementDto(
    long DisbursementId,
    long SrfId,
    long StipendId,
    DateTime DisbursementDate,
    decimal DisbursementAmount,
    string DisbursementStatus,
    string? MonthYear,
    string? BankReference,
    string? ReferenceNo,
    long CreatedBy,
    DateTime CreatedOn
);

public record ProcessMonthlyStipendResultDto(
    string MonthYear,
    int RowsProcessed,
    bool Success,
    string? Message
);

public record CalculateDisbursementResultDto(
    string MonthYear,
    int RowsCreated,
    bool Success,
    string? Message
);
