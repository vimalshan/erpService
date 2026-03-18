using MediatR;
using PayrollServices.Application.DTOs;

namespace PayrollServices.Application.Queries;

/// <summary>
/// Query to get payroll batch by ID
/// </summary>
public class GetPayrollBatchByIdQuery : IRequest<PayrollBatchDto?>
{
    public long BatchId { get; set; }
}
