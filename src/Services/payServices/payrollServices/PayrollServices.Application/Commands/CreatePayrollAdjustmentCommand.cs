using MediatR;
using PayrollServices.Application.DTOs;

namespace PayrollServices.Application.Commands;

/// <summary>
/// Command to create a payroll adjustment (allowance, deduction, arrear)
/// </summary>
public class CreatePayrollAdjustmentCommand : IRequest<PayrollAdjustmentDto>
{
    public long EmployeeSystemId { get; set; }
    public decimal Amount { get; set; }
    public string AdjustmentType { get; set; } = null!; // A=Allowance, D=Deduction, R=Arrear
    public string? Description { get; set; }
    public long CreatedBy { get; set; }
}
