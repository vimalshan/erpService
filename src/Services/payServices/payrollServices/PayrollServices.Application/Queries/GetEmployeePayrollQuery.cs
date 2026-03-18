using MediatR;
using PayrollServices.Application.DTOs;

namespace PayrollServices.Application.Queries;

/// <summary>
/// Query to get payroll transactions by employee and month
/// </summary>
public class GetEmployeePayrollQuery : IRequest<IEnumerable<PayrollTransactionDto>>
{
    public long EmployeeSystemId { get; set; }
    public string? Month { get; set; }
}
