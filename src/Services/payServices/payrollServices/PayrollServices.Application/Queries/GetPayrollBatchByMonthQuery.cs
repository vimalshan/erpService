using MediatR;
using PayrollServices.Application.DTOs;

namespace PayrollServices.Application.Queries;

/// <summary>
/// Query to get payroll batch by month
/// </summary>
public class GetPayrollBatchByMonthQuery : IRequest<PayrollBatchDto?>
{
    public string Month { get; set; } = null!;
}
