using DealTicketing.Application.DTOs;
using MediatR;

namespace DealTicketing.Application.Features.DealDetails.Commands;

public record CreateDealDetailCommand(
    long DealId,
    long DealNo,
    long DealVersionId,
    long DealBatchId,
    char? DealTranType,
    string? DealPosition,
    decimal? DealAmount,
    long? DealBankId,
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
    string? DealRemarks,
    string? DealIrType,
    DateTime? DealStartDate,
    decimal? DealLoanAmt,
    long? DealLoanCurrency,
    decimal ModifiedBy) : IRequest<DealDetailDto>;

public record ApproveDealCommand(
    long DealId,
    long AppBusiness,
    string? Remarks,
    decimal ModifiedBy) : IRequest<Unit>;

public record RejectDealCommand(
    long DealId,
    string Remarks,
    decimal ModifiedBy) : IRequest<Unit>;

public record CreateDealSettlementCommand(
    long SetId,
    long DealId,
    decimal GainLossAmt,
    char? SetType,
    decimal? SpotRate,
    decimal? ExchangeRate,
    long? ModifiedBy) : IRequest<DealSettlementDto>;
