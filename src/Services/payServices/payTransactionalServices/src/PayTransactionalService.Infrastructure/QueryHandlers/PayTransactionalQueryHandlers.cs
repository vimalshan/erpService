using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PayTransactionalService.Application.Common;
using PayTransactionalService.Application.DTOs;
using PayTransactionalService.Application.Queries;
using PayTransactionalService.Domain.Repositories;

namespace PayTransactionalService.Infrastructure.QueryHandlers;

public class GetPayTransactionByIdHandler : IRequestHandler<GetPayTransactionByIdQuery, Result<PayTransactionDto>>
{
    private readonly IPayTransactionRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPayTransactionByIdHandler> _logger;

    public GetPayTransactionByIdHandler(IPayTransactionRepository repo, IMapper mapper, ILogger<GetPayTransactionByIdHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayTransactionDto>> Handle(GetPayTransactionByIdQuery request, CancellationToken ct)
    {
        try
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity == null) return Result<PayTransactionDto>.Failure("Pay transaction not found");
            return Result<PayTransactionDto>.Success(_mapper.Map<PayTransactionDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting pay transaction"); return Result<PayTransactionDto>.Failure(ex.Message); }
    }
}

public class GetPayTransactionsByEmployeeHandler : IRequestHandler<GetPayTransactionsByEmployeeQuery, Result<IEnumerable<PayTransactionDto>>>
{
    private readonly IPayTransactionRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPayTransactionsByEmployeeHandler> _logger;

    public GetPayTransactionsByEmployeeHandler(IPayTransactionRepository repo, IMapper mapper, ILogger<GetPayTransactionsByEmployeeHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<IEnumerable<PayTransactionDto>>> Handle(GetPayTransactionsByEmployeeQuery request, CancellationToken ct)
    {
        try
        {
            var entities = await _repo.GetByEmployeeAsync(request.EmployeeSystemId, ct);
            return Result<IEnumerable<PayTransactionDto>>.Success(_mapper.Map<IEnumerable<PayTransactionDto>>(entities));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting pay transactions"); return Result<IEnumerable<PayTransactionDto>>.Failure(ex.Message); }
    }
}

public class GetPayTransactionsByMonthHandler : IRequestHandler<GetPayTransactionsByMonthQuery, Result<IEnumerable<PayTransactionDto>>>
{
    private readonly IPayTransactionRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPayTransactionsByMonthHandler> _logger;

    public GetPayTransactionsByMonthHandler(IPayTransactionRepository repo, IMapper mapper, ILogger<GetPayTransactionsByMonthHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<IEnumerable<PayTransactionDto>>> Handle(GetPayTransactionsByMonthQuery request, CancellationToken ct)
    {
        try
        {
            var entities = await _repo.GetByMonthYearAsync(request.MonthYear, ct);
            return Result<IEnumerable<PayTransactionDto>>.Success(_mapper.Map<IEnumerable<PayTransactionDto>>(entities));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting pay transactions by month"); return Result<IEnumerable<PayTransactionDto>>.Failure(ex.Message); }
    }
}

public class GetPayTransactionsByBatchHandler : IRequestHandler<GetPayTransactionsByBatchQuery, Result<IEnumerable<PayTransactionDto>>>
{
    private readonly IPayTransactionRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPayTransactionsByBatchHandler> _logger;

    public GetPayTransactionsByBatchHandler(IPayTransactionRepository repo, IMapper mapper, ILogger<GetPayTransactionsByBatchHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<IEnumerable<PayTransactionDto>>> Handle(GetPayTransactionsByBatchQuery request, CancellationToken ct)
    {
        try
        {
            var entities = await _repo.GetByBatchIdAsync(request.BatchId, ct);
            return Result<IEnumerable<PayTransactionDto>>.Success(_mapper.Map<IEnumerable<PayTransactionDto>>(entities));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting batch transactions"); return Result<IEnumerable<PayTransactionDto>>.Failure(ex.Message); }
    }
}

// Pay Arrear Query Handlers
public class GetPayArrearByIdHandler : IRequestHandler<GetPayArrearByIdQuery, Result<PayArrearDto>>
{
    private readonly IPayArrearRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPayArrearByIdHandler> _logger;

    public GetPayArrearByIdHandler(IPayArrearRepository repo, IMapper mapper, ILogger<GetPayArrearByIdHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayArrearDto>> Handle(GetPayArrearByIdQuery request, CancellationToken ct)
    {
        try
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity == null) return Result<PayArrearDto>.Failure("Pay arrear not found");
            return Result<PayArrearDto>.Success(_mapper.Map<PayArrearDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting pay arrear"); return Result<PayArrearDto>.Failure(ex.Message); }
    }
}

public class GetPayArrearsByEmployeeHandler : IRequestHandler<GetPayArrearsByEmployeeQuery, Result<IEnumerable<PayArrearDto>>>
{
    private readonly IPayArrearRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPayArrearsByEmployeeHandler> _logger;

    public GetPayArrearsByEmployeeHandler(IPayArrearRepository repo, IMapper mapper, ILogger<GetPayArrearsByEmployeeHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<IEnumerable<PayArrearDto>>> Handle(GetPayArrearsByEmployeeQuery request, CancellationToken ct)
    {
        try
        {
            var entities = await _repo.GetByEmployeeAsync(request.EmployeeSystemId, ct);
            return Result<IEnumerable<PayArrearDto>>.Success(_mapper.Map<IEnumerable<PayArrearDto>>(entities));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting pay arrears"); return Result<IEnumerable<PayArrearDto>>.Failure(ex.Message); }
    }
}

public class GetPayArrearsByTypeHandler : IRequestHandler<GetPayArrearsByTypeQuery, Result<IEnumerable<PayArrearDto>>>
{
    private readonly IPayArrearRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPayArrearsByTypeHandler> _logger;

    public GetPayArrearsByTypeHandler(IPayArrearRepository repo, IMapper mapper, ILogger<GetPayArrearsByTypeHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<IEnumerable<PayArrearDto>>> Handle(GetPayArrearsByTypeQuery request, CancellationToken ct)
    {
        try
        {
            var entities = await _repo.GetByTypeAsync(request.Type, request.MonthYear, ct);
            return Result<IEnumerable<PayArrearDto>>.Success(_mapper.Map<IEnumerable<PayArrearDto>>(entities));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting arrears by type"); return Result<IEnumerable<PayArrearDto>>.Failure(ex.Message); }
    }
}

public class GetUnprocessedArrearsHandler : IRequestHandler<GetUnprocessedArrearsQuery, Result<IEnumerable<PayArrearDto>>>
{
    private readonly IPayArrearRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUnprocessedArrearsHandler> _logger;

    public GetUnprocessedArrearsHandler(IPayArrearRepository repo, IMapper mapper, ILogger<GetUnprocessedArrearsHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<IEnumerable<PayArrearDto>>> Handle(GetUnprocessedArrearsQuery request, CancellationToken ct)
    {
        try
        {
            var entities = await _repo.GetUnprocessedByEmployeeAsync(request.EmployeeSystemId, ct);
            return Result<IEnumerable<PayArrearDto>>.Success(_mapper.Map<IEnumerable<PayArrearDto>>(entities));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting unprocessed arrears"); return Result<IEnumerable<PayArrearDto>>.Failure(ex.Message); }
    }
}

// Pay Adjustment Query Handlers
public class GetPayAdjustmentByIdHandler : IRequestHandler<GetPayAdjustmentByIdQuery, Result<PayAdjustmentDto>>
{
    private readonly IPayAdjustmentRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPayAdjustmentByIdHandler> _logger;

    public GetPayAdjustmentByIdHandler(IPayAdjustmentRepository repo, IMapper mapper, ILogger<GetPayAdjustmentByIdHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayAdjustmentDto>> Handle(GetPayAdjustmentByIdQuery request, CancellationToken ct)
    {
        try
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity == null) return Result<PayAdjustmentDto>.Failure("Pay adjustment not found");
            return Result<PayAdjustmentDto>.Success(_mapper.Map<PayAdjustmentDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting pay adjustment"); return Result<PayAdjustmentDto>.Failure(ex.Message); }
    }
}

public class GetPayAdjustmentsByEmployeeHandler : IRequestHandler<GetPayAdjustmentsByEmployeeQuery, Result<IEnumerable<PayAdjustmentDto>>>
{
    private readonly IPayAdjustmentRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPayAdjustmentsByEmployeeHandler> _logger;

    public GetPayAdjustmentsByEmployeeHandler(IPayAdjustmentRepository repo, IMapper mapper, ILogger<GetPayAdjustmentsByEmployeeHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<IEnumerable<PayAdjustmentDto>>> Handle(GetPayAdjustmentsByEmployeeQuery request, CancellationToken ct)
    {
        try
        {
            var entities = await _repo.GetByEmployeeAsync(request.EmployeeSystemId, ct);
            return Result<IEnumerable<PayAdjustmentDto>>.Success(_mapper.Map<IEnumerable<PayAdjustmentDto>>(entities));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting adjustments"); return Result<IEnumerable<PayAdjustmentDto>>.Failure(ex.Message); }
    }
}

public class GetPendingAdjustmentsHandler : IRequestHandler<GetPendingAdjustmentsQuery, Result<IEnumerable<PayAdjustmentDto>>>
{
    private readonly IPayAdjustmentRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPendingAdjustmentsHandler> _logger;

    public GetPendingAdjustmentsHandler(IPayAdjustmentRepository repo, IMapper mapper, ILogger<GetPendingAdjustmentsHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<IEnumerable<PayAdjustmentDto>>> Handle(GetPendingAdjustmentsQuery request, CancellationToken ct)
    {
        try
        {
            var entities = await _repo.GetPendingAsync(ct);
            return Result<IEnumerable<PayAdjustmentDto>>.Success(_mapper.Map<IEnumerable<PayAdjustmentDto>>(entities));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting pending adjustments"); return Result<IEnumerable<PayAdjustmentDto>>.Failure(ex.Message); }
    }
}

// Payroll Batch Query Handlers
public class GetPayrollBatchByIdHandler : IRequestHandler<GetPayrollBatchByIdQuery, Result<PayrollBatchDto>>
{
    private readonly IPayrollBatchRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPayrollBatchByIdHandler> _logger;

    public GetPayrollBatchByIdHandler(IPayrollBatchRepository repo, IMapper mapper, ILogger<GetPayrollBatchByIdHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<PayrollBatchDto>> Handle(GetPayrollBatchByIdQuery request, CancellationToken ct)
    {
        try
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity == null) return Result<PayrollBatchDto>.Failure("Payroll batch not found");
            return Result<PayrollBatchDto>.Success(_mapper.Map<PayrollBatchDto>(entity));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting payroll batch"); return Result<PayrollBatchDto>.Failure(ex.Message); }
    }
}

public class GetAllPayrollBatchesHandler : IRequestHandler<GetAllPayrollBatchesQuery, Result<IEnumerable<PayrollBatchDto>>>
{
    private readonly IPayrollBatchRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllPayrollBatchesHandler> _logger;

    public GetAllPayrollBatchesHandler(IPayrollBatchRepository repo, IMapper mapper, ILogger<GetAllPayrollBatchesHandler> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    public async Task<Result<IEnumerable<PayrollBatchDto>>> Handle(GetAllPayrollBatchesQuery request, CancellationToken ct)
    {
        try
        {
            var entities = await _repo.GetAllAsync(ct);
            return Result<IEnumerable<PayrollBatchDto>>.Success(_mapper.Map<IEnumerable<PayrollBatchDto>>(entities));
        }
        catch (Exception ex) { _logger.LogError(ex, "Error getting payroll batches"); return Result<IEnumerable<PayrollBatchDto>>.Failure(ex.Message); }
    }
}
