using MediatR;
using PayTransactionalService.Application.Common;
using PayTransactionalService.Application.DTOs;

namespace PayTransactionalService.Application.Queries;

// Pay Transaction queries
public record GetPayTransactionByIdQuery(long Id) : IRequest<Result<PayTransactionDto>>;
public record GetPayTransactionsByEmployeeQuery(long EmployeeSystemId) : IRequest<Result<IEnumerable<PayTransactionDto>>>;
public record GetPayTransactionsByMonthQuery(string MonthYear) : IRequest<Result<IEnumerable<PayTransactionDto>>>;
public record GetPayTransactionsByBatchQuery(long BatchId) : IRequest<Result<IEnumerable<PayTransactionDto>>>;

// Pay Arrear queries
public record GetPayArrearByIdQuery(long Id) : IRequest<Result<PayArrearDto>>;
public record GetPayArrearsByEmployeeQuery(long EmployeeSystemId) : IRequest<Result<IEnumerable<PayArrearDto>>>;
public record GetPayArrearsByTypeQuery(string Type, string? MonthYear = null) : IRequest<Result<IEnumerable<PayArrearDto>>>;
public record GetUnprocessedArrearsQuery(long EmployeeSystemId) : IRequest<Result<IEnumerable<PayArrearDto>>>;

// Pay Adjustment queries
public record GetPayAdjustmentByIdQuery(long Id) : IRequest<Result<PayAdjustmentDto>>;
public record GetPayAdjustmentsByEmployeeQuery(long EmployeeSystemId) : IRequest<Result<IEnumerable<PayAdjustmentDto>>>;
public record GetPendingAdjustmentsQuery() : IRequest<Result<IEnumerable<PayAdjustmentDto>>>;

// Payroll Batch queries
public record GetPayrollBatchByIdQuery(long Id) : IRequest<Result<PayrollBatchDto>>;
public record GetAllPayrollBatchesQuery() : IRequest<Result<IEnumerable<PayrollBatchDto>>>;
