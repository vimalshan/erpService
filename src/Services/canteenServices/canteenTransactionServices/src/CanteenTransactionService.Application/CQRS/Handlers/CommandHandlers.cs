using MediatR;
using CanteenTransactionService.Application.CQRS.Commands;
using CanteenTransactionService.Application.DTOs;
using CanteenTransactionService.Domain.Entities;
using CanteenTransactionService.Domain.Exceptions;
using CanteenTransactionService.Domain.Interfaces;

namespace CanteenTransactionService.Application.CQRS.Handlers;

// ---- CanteenDacon Command Handlers ----

public class RecordCanteenTransactionCommandHandler : IRequestHandler<RecordCanteenTransactionCommand, CanteenDaconDto>
{
    private readonly ICanteenDaconRepository _repo;

    public RecordCanteenTransactionCommandHandler(ICanteenDaconRepository repo) => _repo = repo;

    public async Task<CanteenDaconDto> Handle(RecordCanteenTransactionCommand request, CancellationToken ct)
    {
        var serialNumber = await _repo.GetNextSerialNumberAsync(ct);

        var entity = CanteenDacon.Record(
            serialNumber,
            request.CompanyCode,
            request.EmployeeSysId,
            request.EmployeeType,
            request.SwipeDate,
            request.ItemCode,
            request.ItemType,
            request.EmployeeContribution,
            request.EmployerContribution,
            request.CanteenNumber,
            request.ItemQuantity,
            request.EntryUser,
            request.GradeCategory);

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        return MapToDto(entity);
    }

    internal static CanteenDaconDto MapToDto(CanteenDacon e) => new(
        e.SerialNumber, e.CompanyCode, e.EmployeeSysId, e.EmployeeType,
        e.SwipeDate, e.ItemCode, e.ItemType, e.EmployeeContribution,
        e.EmployerContribution, e.CanteenNumber, e.ItemQuantity,
        e.EntryUser, e.EntryDate, e.GradeCategory);
}

public class CancelCanteenTransactionCommandHandler : IRequestHandler<CancelCanteenTransactionCommand, bool>
{
    private readonly ICanteenDaconRepository _repo;

    public CancelCanteenTransactionCommandHandler(ICanteenDaconRepository repo) => _repo = repo;

    public async Task<bool> Handle(CancelCanteenTransactionCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetBySerialNumberAsync(request.SerialNumber, ct)
            ?? throw new TransactionNotFoundException(request.SerialNumber);

        entity.Cancel();
        _repo.Delete(entity);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}

// ---- DailyAvailed Command Handlers ----

public class ProcessDailyAvailedCommandHandler : IRequestHandler<ProcessDailyAvailedCommand, DailyAvailedDto>
{
    private readonly IDailyAvailedRepository _repo;

    public ProcessDailyAvailedCommandHandler(IDailyAvailedRepository repo) => _repo = repo;

    public async Task<DailyAvailedDto> Handle(ProcessDailyAvailedCommand request, CancellationToken ct)
    {
        var serialNumber = await _repo.GetNextSerialNumberAsync(ct);

        var entity = DailyAvailed.Create(
            serialNumber,
            request.CompanyCode,
            request.EmployeeSysId,
            request.EmployeeType,
            request.SwipeDate,
            request.ItemCode,
            request.ItemType,
            request.EmployeeContribution,
            request.EmployerContribution,
            request.CanteenNumber,
            request.ItemQuantity,
            request.EntryUser,
            request.GradeCategory);

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        return MapAvailedDto(entity);
    }

    internal static DailyAvailedDto MapAvailedDto(DailyAvailed e) => new(
        e.SerialNumber, e.CompanyCode, e.EmployeeSysId, e.EmployeeType,
        e.SwipeDate, e.ItemCode, e.ItemType, e.EmployeeContribution,
        e.EmployerContribution, e.CanteenNumber, e.ItemQuantity,
        e.EntryUser, e.EntryDate, e.GradeCategory);
}

// ---- MIS Batch Command Handlers ----

public class SubmitMisBatchCommandHandler : IRequestHandler<SubmitMisBatchCommand, MisBatchSubmissionDto>
{
    private readonly IMisBatchSubmissionRepository _repo;

    public SubmitMisBatchCommandHandler(IMisBatchSubmissionRepository repo) => _repo = repo;

    public async Task<MisBatchSubmissionDto> Handle(SubmitMisBatchCommand request, CancellationToken ct)
    {
        var entity = MisBatchSubmission.Create(
            request.CompanyCode,
            request.EmployeeNumber,
            request.SwipeTime,
            request.ItemCode,
            request.ItemQuantity,
            request.BatchDate,
            request.BatchNumber,
            request.BatchNumber, // serial = batch for new submissions
            request.CanteenNumber,
            request.GateNumber);

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        return MapBatchDto(entity);
    }

    internal static MisBatchSubmissionDto MapBatchDto(MisBatchSubmission e) => new(
        e.CompanyCode, e.EmployeeNumber, e.SwipeTime, e.ItemCode,
        e.ItemQuantity, e.BatchDate, e.BatchNumber, e.SerialNumber,
        e.EntryDate, e.CanteenNumber, e.GateNumber, e.UpdateStatus);
}

public class ProcessMisBatchCommandHandler : IRequestHandler<ProcessMisBatchCommand, bool>
{
    private readonly IMisBatchSubmissionRepository _repo;

    public ProcessMisBatchCommandHandler(IMisBatchSubmissionRepository repo) => _repo = repo;

    public async Task<bool> Handle(ProcessMisBatchCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetBySerialNumberAsync(request.SerialNumber, ct)
            ?? throw new BatchNotFoundException(request.SerialNumber);

        entity.MarkAsProcessed();
        _repo.Update(entity);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}

public class FailMisBatchCommandHandler : IRequestHandler<FailMisBatchCommand, bool>
{
    private readonly IMisBatchSubmissionRepository _repo;

    public FailMisBatchCommandHandler(IMisBatchSubmissionRepository repo) => _repo = repo;

    public async Task<bool> Handle(FailMisBatchCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetBySerialNumberAsync(request.SerialNumber, ct)
            ?? throw new BatchNotFoundException(request.SerialNumber);

        entity.MarkAsFailed();
        _repo.Update(entity);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
