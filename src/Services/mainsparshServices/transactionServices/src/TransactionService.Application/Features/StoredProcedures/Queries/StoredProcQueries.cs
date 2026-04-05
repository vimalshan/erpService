using MediatR;
using TransactionService.Application.DTOs;

namespace TransactionService.Application.Features.StoredProcedures.Queries;

public record GetPendingApprovalsSpQuery(long? ApproverId = null) : IRequest<StoredProcResultDto>;
public record GetAuditLogSpQuery(string? EntityType = null, long? EntityId = null, DateTime? FromDate = null, DateTime? ToDate = null) : IRequest<StoredProcResultDto>;
public record GetPendingDisbursementsSpQuery() : IRequest<StoredProcResultDto>;
public record GetAvailableRoomsSpQuery(DateTime Date, TimeSpan StartTime, TimeSpan EndTime) : IRequest<StoredProcResultDto>;
public record ValidateBookingAttendeesSpQuery(long BookingId) : IRequest<bool>;
public record CalculateStipendSpQuery(long ResearchCategoryId, long RankId) : IRequest<decimal>;
