using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Features.StoredProcedures.Queries;

public class GetPendingApprovalsSpQueryHandler : IRequestHandler<GetPendingApprovalsSpQuery, StoredProcResultDto>
{
    private readonly ITransactionDapperRepository _dapper;
    public GetPendingApprovalsSpQueryHandler(ITransactionDapperRepository dapper) => _dapper = dapper;

    public async Task<StoredProcResultDto> Handle(GetPendingApprovalsSpQuery request, CancellationToken cancellationToken)
    {
        var data = await _dapper.GetPendingApprovalsAsync(request.ApproverId, cancellationToken);
        return new StoredProcResultDto(true, "Pending approvals retrieved.", data);
    }
}

public class GetAuditLogSpQueryHandler : IRequestHandler<GetAuditLogSpQuery, StoredProcResultDto>
{
    private readonly ITransactionDapperRepository _dapper;
    public GetAuditLogSpQueryHandler(ITransactionDapperRepository dapper) => _dapper = dapper;

    public async Task<StoredProcResultDto> Handle(GetAuditLogSpQuery request, CancellationToken cancellationToken)
    {
        var data = await _dapper.GetAuditLogAsync(request.EntityType, request.EntityId, request.FromDate, request.ToDate, cancellationToken);
        return new StoredProcResultDto(true, "Audit log retrieved.", data);
    }
}

public class GetPendingDisbursementsSpQueryHandler : IRequestHandler<GetPendingDisbursementsSpQuery, StoredProcResultDto>
{
    private readonly ITransactionDapperRepository _dapper;
    public GetPendingDisbursementsSpQueryHandler(ITransactionDapperRepository dapper) => _dapper = dapper;

    public async Task<StoredProcResultDto> Handle(GetPendingDisbursementsSpQuery request, CancellationToken cancellationToken)
    {
        var data = await _dapper.GetPendingDisbursementsAsync(cancellationToken);
        return new StoredProcResultDto(true, "Pending disbursements retrieved.", data);
    }
}

public class GetAvailableRoomsSpQueryHandler : IRequestHandler<GetAvailableRoomsSpQuery, StoredProcResultDto>
{
    private readonly ITransactionDapperRepository _dapper;
    public GetAvailableRoomsSpQueryHandler(ITransactionDapperRepository dapper) => _dapper = dapper;

    public async Task<StoredProcResultDto> Handle(GetAvailableRoomsSpQuery request, CancellationToken cancellationToken)
    {
        var data = await _dapper.GetAvailableRoomsAsync(request.Date, request.StartTime, request.EndTime, cancellationToken);
        return new StoredProcResultDto(true, "Available rooms retrieved.", data);
    }
}

public class ValidateBookingAttendeesSpQueryHandler : IRequestHandler<ValidateBookingAttendeesSpQuery, bool>
{
    private readonly ITransactionDapperRepository _dapper;
    public ValidateBookingAttendeesSpQueryHandler(ITransactionDapperRepository dapper) => _dapper = dapper;

    public async Task<bool> Handle(ValidateBookingAttendeesSpQuery request, CancellationToken cancellationToken)
    {
        return await _dapper.ValidateBookingAttendeesAsync(request.BookingId, cancellationToken);
    }
}

public class CalculateStipendSpQueryHandler : IRequestHandler<CalculateStipendSpQuery, decimal>
{
    private readonly ITransactionDapperRepository _dapper;
    public CalculateStipendSpQueryHandler(ITransactionDapperRepository dapper) => _dapper = dapper;

    public async Task<decimal> Handle(CalculateStipendSpQuery request, CancellationToken cancellationToken)
    {
        return await _dapper.CalculateSRFStipendAsync(request.ResearchCategoryId, request.RankId, cancellationToken);
    }
}
