namespace DealTicketing.Application.DTOs;

public record DealBatchDto(
    long DealBatchId,
    DateTime DealDate,
    long DealDerType,
    string? DealScreenshot,
    long? DealBookedBy,
    string? DealBankTrader,
    long? DealBankId,
    string? BankName,
    long? DealOptionType,
    decimal DealBusinessId,
    char? DealRejStatus,
    string? DealRejReason,
    string? DealErrRemarks,
    decimal DealModifiedBy,
    DateTime DealModifiedOn,
    decimal? DealUnitId,
    int DealCount = 0);

public record DealDetailDto(
    long DealId,
    long DealNo,
    long DealVersionId,
    long DealBatchId,
    char? DealTranType,
    string? DealPosition,
    DateTime? DealEntryDate,
    decimal? DealAmount,
    long? DealBankId,
    string? BankName,
    long? DealCurrency1,
    long? DealCurrency2,
    decimal? DealSpotRate,
    decimal? DealForPoints,
    decimal? DealBankMargin,
    decimal? DealBookRate,
    DateTime? DealMatDate,
    long? DealDealType,
    long? DealBusiness,
    long? DealCategory,
    char? DealAppStatus,
    string? DealAppRemarks,
    char? DealSetStatus,
    string? DealRemarks,
    string? DealIrType,
    DateTime? DealStartDate,
    decimal? DealLoanAmt,
    DateTime? DealModifiedOn,
    decimal? DealModifiedBy);

public record DealSettlementDto(
    long SetId,
    long SetDealId,
    decimal? SetSpotRate,
    DateTime? SetDate,
    string? SetMoneyType,
    char? SetExcType,
    decimal SetGainLossAmt,
    char? SetType,
    decimal? SetExchangeRate,
    decimal? SetActGainLossAmt,
    decimal? SetAmount,
    decimal? SetCreditDebit,
    string? SetBankName,
    string? SetBankAcNo,
    DateTime? SetModifiedOn);

public record BankDto(
    long BankId,
    string BankName,
    DateTime BankEffDate,
    DateTime? BankClsDate);

public record LovMasterDto(
    long LovId,
    string LovType,
    string LovName);

public record DealSummaryDto(
    DateTime DealDate,
    string BankName,
    int DealCount,
    decimal TotalAmount,
    int ConfirmedDeals,
    int PendingDeals,
    int RejectedDeals);
