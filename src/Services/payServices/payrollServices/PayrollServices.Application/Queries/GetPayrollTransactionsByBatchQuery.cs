using MediatR;
using PayrollServices.Application.DTOs;

namespace PayrollServices.Application.Queries;

/// <summary>
/// Query to get payroll transactions by batch
/// </summary>
public class GetPayrollTransactionsByBatchQuery : IRequest<IEnumerable<PayrollTransactionDto>>
{
    public long BatchId { get; set; }
}
