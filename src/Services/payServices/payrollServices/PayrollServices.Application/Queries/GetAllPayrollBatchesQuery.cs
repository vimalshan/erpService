using MediatR;
using PayrollServices.Application.DTOs;

namespace PayrollServices.Application.Queries;

/// <summary>
/// Query to get all payroll batches
/// </summary>
public class GetAllPayrollBatchesQuery : IRequest<IEnumerable<PayrollBatchDto>>
{
}
