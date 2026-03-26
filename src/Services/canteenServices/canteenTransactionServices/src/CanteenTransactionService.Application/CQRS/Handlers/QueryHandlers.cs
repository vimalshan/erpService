using MediatR;
using CanteenTransactionService.Application.CQRS.Queries;
using CanteenTransactionService.Application.DTOs;
using CanteenTransactionService.Domain.Interfaces;

namespace CanteenTransactionService.Application.CQRS.Handlers;

// ---- CanteenDacon Query Handlers ----

public class GetTransactionBySerialNumberQueryHandler : IRequestHandler<GetTransactionBySerialNumberQuery, CanteenDaconDto?>
{
    private readonly ICanteenDaconRepository _repo;

    public GetTransactionBySerialNumberQueryHandler(ICanteenDaconRepository repo) => _repo = repo;

    public async Task<CanteenDaconDto?> Handle(GetTransactionBySerialNumberQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetBySerialNumberAsync(request.SerialNumber, ct);
        return entity is null ? null : RecordCanteenTransactionCommandHandler.MapToDto(entity);
    }
}

public class GetTransactionsByEmployeeQueryHandler : IRequestHandler<GetTransactionsByEmployeeQuery, IEnumerable<CanteenDaconDto>>
{
    private readonly ICanteenDaconRepository _repo;

    public GetTransactionsByEmployeeQueryHandler(ICanteenDaconRepository repo) => _repo = repo;

    public async Task<IEnumerable<CanteenDaconDto>> Handle(GetTransactionsByEmployeeQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByEmployeeAsync(request.EmployeeSysId, request.FromDate, request.ToDate, ct);
        return items.Select(RecordCanteenTransactionCommandHandler.MapToDto);
    }
}

public class GetTransactionsByCompanyAndDateQueryHandler : IRequestHandler<GetTransactionsByCompanyAndDateQuery, IEnumerable<CanteenDaconDto>>
{
    private readonly ICanteenDaconRepository _repo;

    public GetTransactionsByCompanyAndDateQueryHandler(ICanteenDaconRepository repo) => _repo = repo;

    public async Task<IEnumerable<CanteenDaconDto>> Handle(GetTransactionsByCompanyAndDateQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByCompanyAndDateAsync(request.CompanyCode, request.SwipeDate, ct);
        return items.Select(RecordCanteenTransactionCommandHandler.MapToDto);
    }
}

// ---- DailyAvailed Query Handlers ----

public class GetDailyAvailedBySerialNumberQueryHandler : IRequestHandler<GetDailyAvailedBySerialNumberQuery, DailyAvailedDto?>
{
    private readonly IDailyAvailedRepository _repo;

    public GetDailyAvailedBySerialNumberQueryHandler(IDailyAvailedRepository repo) => _repo = repo;

    public async Task<DailyAvailedDto?> Handle(GetDailyAvailedBySerialNumberQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetBySerialNumberAsync(request.SerialNumber, ct);
        return entity is null ? null : ProcessDailyAvailedCommandHandler.MapAvailedDto(entity);
    }
}

public class GetDailyAvailedByEmployeeQueryHandler : IRequestHandler<GetDailyAvailedByEmployeeQuery, IEnumerable<DailyAvailedDto>>
{
    private readonly IDailyAvailedRepository _repo;

    public GetDailyAvailedByEmployeeQueryHandler(IDailyAvailedRepository repo) => _repo = repo;

    public async Task<IEnumerable<DailyAvailedDto>> Handle(GetDailyAvailedByEmployeeQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByEmployeeAsync(request.EmployeeSysId, request.FromDate, request.ToDate, ct);
        return items.Select(ProcessDailyAvailedCommandHandler.MapAvailedDto);
    }
}

public class GetDailyAvailedByCompanyAndDateQueryHandler : IRequestHandler<GetDailyAvailedByCompanyAndDateQuery, IEnumerable<DailyAvailedDto>>
{
    private readonly IDailyAvailedRepository _repo;

    public GetDailyAvailedByCompanyAndDateQueryHandler(IDailyAvailedRepository repo) => _repo = repo;

    public async Task<IEnumerable<DailyAvailedDto>> Handle(GetDailyAvailedByCompanyAndDateQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByCompanyAndDateAsync(request.CompanyCode, request.SwipeDate, ct);
        return items.Select(ProcessDailyAvailedCommandHandler.MapAvailedDto);
    }
}

// ---- MIS Batch Query Handlers ----

public class GetMisBatchBySerialNumberQueryHandler : IRequestHandler<GetMisBatchBySerialNumberQuery, MisBatchSubmissionDto?>
{
    private readonly IMisBatchSubmissionRepository _repo;

    public GetMisBatchBySerialNumberQueryHandler(IMisBatchSubmissionRepository repo) => _repo = repo;

    public async Task<MisBatchSubmissionDto?> Handle(GetMisBatchBySerialNumberQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetBySerialNumberAsync(request.SerialNumber, ct);
        return entity is null ? null : SubmitMisBatchCommandHandler.MapBatchDto(entity);
    }
}

public class GetMisBatchByBatchNumberQueryHandler : IRequestHandler<GetMisBatchByBatchNumberQuery, IEnumerable<MisBatchSubmissionDto>>
{
    private readonly IMisBatchSubmissionRepository _repo;

    public GetMisBatchByBatchNumberQueryHandler(IMisBatchSubmissionRepository repo) => _repo = repo;

    public async Task<IEnumerable<MisBatchSubmissionDto>> Handle(GetMisBatchByBatchNumberQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByBatchNumberAsync(request.BatchNumber, ct);
        return items.Select(SubmitMisBatchCommandHandler.MapBatchDto);
    }
}

public class GetPendingMisBatchesQueryHandler : IRequestHandler<GetPendingMisBatchesQuery, IEnumerable<MisBatchSubmissionDto>>
{
    private readonly IMisBatchSubmissionRepository _repo;

    public GetPendingMisBatchesQueryHandler(IMisBatchSubmissionRepository repo) => _repo = repo;

    public async Task<IEnumerable<MisBatchSubmissionDto>> Handle(GetPendingMisBatchesQuery request, CancellationToken ct)
    {
        var items = await _repo.GetPendingAsync(ct);
        return items.Select(SubmitMisBatchCommandHandler.MapBatchDto);
    }
}
