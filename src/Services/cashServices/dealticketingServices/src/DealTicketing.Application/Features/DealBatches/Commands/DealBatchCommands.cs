using DealTicketing.Application.DTOs;
using MediatR;

namespace DealTicketing.Application.Features.DealBatches.Commands;

public record CreateDealBatchCommand(
    long DealBatchId,
    DateTime DealDate,
    long DealDerType,
    long? DealBankId,
    long? DealBookedBy,
    string? DealBankTrader,
    decimal DealBusinessId,
    decimal DealModifiedBy,
    decimal? DealUnitId,
    long? DealOptionType) : IRequest<DealBatchDto>;

public record RejectDealBatchCommand(
    long DealBatchId,
    string RejectionReason,
    decimal ModifiedBy) : IRequest<Unit>;

public record UpdateDealBatchScreenshotCommand(
    long DealBatchId,
    string Screenshot,
    decimal ModifiedBy) : IRequest<Unit>;
