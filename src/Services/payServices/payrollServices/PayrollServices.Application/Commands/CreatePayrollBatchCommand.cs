using MediatR;
using PayrollServices.Application.DTOs;

namespace PayrollServices.Application.Commands;

/// <summary>
/// Command to create a payroll batch
/// </summary>
public class CreatePayrollBatchCommand : IRequest<PayrollBatchDto>
{
    public string BatchMonth { get; set; } = null!;
    public long CreatedBy { get; set; }
}
