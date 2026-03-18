using MediatR;
using PayrollServices.Application.DTOs;

namespace PayrollServices.Application.Commands;

/// <summary>
/// Command to create a payroll transaction
/// </summary>
public class CreatePayrollTransactionCommand : IRequest<PayrollTransactionDto>
{
    public long EmployeeSystemId { get; set; }
    public long BatchId { get; set; }
    public string Month { get; set; } = null!;
    public decimal GrossSalary { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetSalary { get; set; }
    public long CreatedBy { get; set; }
}
