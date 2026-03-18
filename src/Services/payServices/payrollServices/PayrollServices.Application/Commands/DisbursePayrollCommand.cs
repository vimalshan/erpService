using MediatR;
using PayrollServices.Application.DTOs;

namespace PayrollServices.Application.Commands;

/// <summary>
/// Command to disburse payroll
/// </summary>
public class DisbursePayrollCommand : IRequest<bool>
{
    public long TransactionId { get; set; }
    public long DisburseBy { get; set; }
}
