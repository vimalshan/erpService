using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PayTransactionalService.Application.Commands;
using PayTransactionalService.Application.Common;
using PayTransactionalService.Application.DTOs;
using PayTransactionalService.Domain.Entities;
using PayTransactionalService.Domain.Repositories;
using PayTransactionalService.Domain.ValueObjects;

namespace PayTransactionalService.Infrastructure.CommandHandlers;

public class CreatePayTransactionHandler : IRequestHandler<CreatePayTransactionCommand, Result<PayTransactionDto>>
{
    private readonly IPayTransactionRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<CreatePayTransactionHandler> _logger;

    public CreatePayTransactionHandler(IPayTransactionRepository repo, IMapper mapper, ILogger<CreatePayTransactionHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayTransactionDto>> Handle(CreatePayTransactionCommand request, CancellationToken ct)
    {
        try
        {
            var entity = PayTransaction.Create(
                request.Detail.EmployeeSystemId,
                request.Detail.MonthYear,
                request.Detail.GrossAmount,
                request.Detail.Deductions,
                request.UserId);
            await _repo.AddAsync(entity, ct);
            return Result<PayTransactionDto>.Success(_mapper.Map<PayTransactionDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error creating pay transaction"); return Result<PayTransactionDto>.Failure(ex.Message); }
    }
}

public class CompletePayTransactionHandler : IRequestHandler<CompletePayTransactionCommand, Result<PayTransactionDto>>
{
    private readonly IPayTransactionRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<CompletePayTransactionHandler> _logger;

    public CompletePayTransactionHandler(IPayTransactionRepository repo, IMapper mapper, ILogger<CompletePayTransactionHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayTransactionDto>> Handle(CompletePayTransactionCommand request, CancellationToken ct)
    {
        try
        {
            var entity = await _repo.GetByIdAsync(request.TransactionId, ct);
            if (entity == null) return Result<PayTransactionDto>.Failure("Pay transaction not found");
            entity.Complete();
            await _repo.UpdateAsync(entity, ct);
            return Result<PayTransactionDto>.Success(_mapper.Map<PayTransactionDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error completing pay transaction"); return Result<PayTransactionDto>.Failure(ex.Message); }
    }
}

public class RevokePayTransactionHandler : IRequestHandler<RevokePayTransactionCommand, Result<PayTransactionDto>>
{
    private readonly IPayTransactionRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<RevokePayTransactionHandler> _logger;

    public RevokePayTransactionHandler(IPayTransactionRepository repo, IMapper mapper, ILogger<RevokePayTransactionHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayTransactionDto>> Handle(RevokePayTransactionCommand request, CancellationToken ct)
    {
        try
        {
            var entity = await _repo.GetByIdAsync(request.TransactionId, ct);
            if (entity == null) return Result<PayTransactionDto>.Failure("Pay transaction not found");
            entity.Revoke(request.RevokedBy, request.Reason);
            await _repo.UpdateAsync(entity, ct);
            return Result<PayTransactionDto>.Success(_mapper.Map<PayTransactionDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error revoking pay transaction"); return Result<PayTransactionDto>.Failure(ex.Message); }
    }
}

// Pay Arrear Handlers
public class CreatePayArrearHandler : IRequestHandler<CreatePayArrearCommand, Result<PayArrearDto>>
{
    private readonly IPayArrearRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<CreatePayArrearHandler> _logger;

    public CreatePayArrearHandler(IPayArrearRepository repo, IMapper mapper, ILogger<CreatePayArrearHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayArrearDto>> Handle(CreatePayArrearCommand request, CancellationToken ct)
    {
        try
        {
            var entity = PayArrear.Create(
                request.Detail.EmployeeSystemId,
                request.Detail.Amount,
                request.Detail.Type,
                request.Detail.MonthYear,
                request.UserId,
                request.Detail.Code,
                request.Detail.Description);
            await _repo.AddAsync(entity, ct);
            return Result<PayArrearDto>.Success(_mapper.Map<PayArrearDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error creating pay arrear"); return Result<PayArrearDto>.Failure(ex.Message); }
    }
}

public class MarkArrearProcessedHandler : IRequestHandler<MarkArrearProcessedCommand, Result<PayArrearDto>>
{
    private readonly IPayArrearRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<MarkArrearProcessedHandler> _logger;

    public MarkArrearProcessedHandler(IPayArrearRepository repo, IMapper mapper, ILogger<MarkArrearProcessedHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayArrearDto>> Handle(MarkArrearProcessedCommand request, CancellationToken ct)
    {
        try
        {
            var entity = await _repo.GetByIdAsync(request.ArrearId, ct);
            if (entity == null) return Result<PayArrearDto>.Failure("Pay arrear not found");
            entity.MarkProcessed();
            await _repo.UpdateAsync(entity, ct);
            return Result<PayArrearDto>.Success(_mapper.Map<PayArrearDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error marking arrear processed"); return Result<PayArrearDto>.Failure(ex.Message); }
    }
}

// Pay Adjustment Handlers
public class CreatePayAdjustmentHandler : IRequestHandler<CreatePayAdjustmentCommand, Result<PayAdjustmentDto>>
{
    private readonly IPayAdjustmentRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<CreatePayAdjustmentHandler> _logger;

    public CreatePayAdjustmentHandler(IPayAdjustmentRepository repo, IMapper mapper, ILogger<CreatePayAdjustmentHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayAdjustmentDto>> Handle(CreatePayAdjustmentCommand request, CancellationToken ct)
    {
        try
        {
            var entity = PayAdjustment.Create(
                request.Detail.EmployeeSystemId,
                request.Detail.AdjustmentType,
                request.Detail.Amount,
                request.Detail.MonthYear,
                request.Detail.EffectiveDate,
                request.UserId,
                request.Detail.Reason);
            await _repo.AddAsync(entity, ct);
            return Result<PayAdjustmentDto>.Success(_mapper.Map<PayAdjustmentDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error creating pay adjustment"); return Result<PayAdjustmentDto>.Failure(ex.Message); }
    }
}

public class ApprovePayAdjustmentHandler : IRequestHandler<ApprovePayAdjustmentCommand, Result<PayAdjustmentDto>>
{
    private readonly IPayAdjustmentRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<ApprovePayAdjustmentHandler> _logger;

    public ApprovePayAdjustmentHandler(IPayAdjustmentRepository repo, IMapper mapper, ILogger<ApprovePayAdjustmentHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayAdjustmentDto>> Handle(ApprovePayAdjustmentCommand request, CancellationToken ct)
    {
        try
        {
            var entity = await _repo.GetByIdAsync(request.AdjustmentId, ct);
            if (entity == null) return Result<PayAdjustmentDto>.Failure("Pay adjustment not found");
            entity.Approve(request.ApprovedBy);
            await _repo.UpdateAsync(entity, ct);
            return Result<PayAdjustmentDto>.Success(_mapper.Map<PayAdjustmentDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error approving pay adjustment"); return Result<PayAdjustmentDto>.Failure(ex.Message); }
    }
}

public class RejectPayAdjustmentHandler : IRequestHandler<RejectPayAdjustmentCommand, Result<PayAdjustmentDto>>
{
    private readonly IPayAdjustmentRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<RejectPayAdjustmentHandler> _logger;

    public RejectPayAdjustmentHandler(IPayAdjustmentRepository repo, IMapper mapper, ILogger<RejectPayAdjustmentHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayAdjustmentDto>> Handle(RejectPayAdjustmentCommand request, CancellationToken ct)
    {
        try
        {
            var entity = await _repo.GetByIdAsync(request.AdjustmentId, ct);
            if (entity == null) return Result<PayAdjustmentDto>.Failure("Pay adjustment not found");
            entity.Reject(request.RejectedBy, request.Reason);
            await _repo.UpdateAsync(entity, ct);
            return Result<PayAdjustmentDto>.Success(_mapper.Map<PayAdjustmentDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error rejecting pay adjustment"); return Result<PayAdjustmentDto>.Failure(ex.Message); }
    }
}

// Payroll Batch Handlers
public class ProcessMonthlySalaryHandler : IRequestHandler<ProcessMonthlySalaryCommand, Result<PayrollBatchDto>>
{
    private readonly IPayrollBatchRepository _batchRepo;
    private readonly IPayTransactionRepository _txnRepo;
    private readonly IPayArrearRepository _arrearRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<ProcessMonthlySalaryHandler> _logger;

    public ProcessMonthlySalaryHandler(
        IPayrollBatchRepository batchRepo,
        IPayTransactionRepository txnRepo,
        IPayArrearRepository arrearRepo,
        IMapper mapper,
        ILogger<ProcessMonthlySalaryHandler> logger)
    { _batchRepo = batchRepo; _txnRepo = txnRepo; _arrearRepo = arrearRepo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayrollBatchDto>> Handle(ProcessMonthlySalaryCommand request, CancellationToken ct)
    {
        try
        {
            // Check if batch already exists
            var existing = await _batchRepo.GetByMonthYearAsync(request.MonthYear, ct);
            if (existing != null)
                return Result<PayrollBatchDto>.Failure($"Batch already exists for {request.MonthYear}");

            var batch = PayrollBatch.Create(request.MonthYear, request.UserId);
            await _batchRepo.AddAsync(batch, ct);

            // Get all transactions for the month and link to batch
            var transactions = await _txnRepo.GetByMonthYearAsync(request.MonthYear, ct);
            var count = 0;
            foreach (var txn in transactions)
            {
                txn.Complete();
                await _txnRepo.UpdateAsync(txn, ct);
                count++;
            }

            batch.Complete(count);
            await _batchRepo.UpdateAsync(batch, ct);

            return Result<PayrollBatchDto>.Success(_mapper.Map<PayrollBatchDto>(batch));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error processing monthly salary"); return Result<PayrollBatchDto>.Failure(ex.Message); }
    }
}

public class RevokePayrollBatchHandler : IRequestHandler<RevokePayrollBatchCommand, Result<PayrollBatchDto>>
{
    private readonly IPayrollBatchRepository _batchRepo;
    private readonly IPayTransactionRepository _txnRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<RevokePayrollBatchHandler> _logger;

    public RevokePayrollBatchHandler(IPayrollBatchRepository batchRepo, IPayTransactionRepository txnRepo, IMapper mapper, ILogger<RevokePayrollBatchHandler> logger)
    { _batchRepo = batchRepo; _txnRepo = txnRepo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayrollBatchDto>> Handle(RevokePayrollBatchCommand request, CancellationToken ct)
    {
        try
        {
            var batch = await _batchRepo.GetByIdAsync(request.BatchId, ct);
            if (batch == null) return Result<PayrollBatchDto>.Failure("Payroll batch not found");

            batch.Revoke(request.RevokedBy);
            await _batchRepo.UpdateAsync(batch, ct);

            // Revoke all transactions in the batch
            var transactions = await _txnRepo.GetByBatchIdAsync(request.BatchId, ct);
            foreach (var txn in transactions)
            {
                txn.Revoke(request.RevokedBy, "Batch revoked");
                await _txnRepo.UpdateAsync(txn, ct);
            }

            return Result<PayrollBatchDto>.Success(_mapper.Map<PayrollBatchDto>(batch));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error revoking payroll batch"); return Result<PayrollBatchDto>.Failure(ex.Message); }
    }
}
