using PayrollServices.Application.Commands;
using PayrollServices.Application.DTOs;
using MediatR;

namespace PayrollServices.API.GraphQL;

/// <summary>
/// GraphQL Mutation type for payroll operations
/// </summary>
public class PayrollMutation
{
    public async Task<PayrollBatchDto> CreatePayrollBatch(
        [Service] IMediator mediator,
        string batchMonth,
        long createdBy)
    {
        var command = new CreatePayrollBatchCommand
        {
            BatchMonth = batchMonth,
            CreatedBy = createdBy
        };
        return await mediator.Send(command);
    }

    public async Task<PayrollTransactionDto> CreatePayrollTransaction(
        [Service] IMediator mediator,
        long employeeSystemId,
        long batchId,
        string month,
        decimal grossSalary,
        decimal deductions,
        decimal netSalary,
        long createdBy)
    {
        var command = new CreatePayrollTransactionCommand
        {
            EmployeeSystemId = employeeSystemId,
            BatchId = batchId,
            Month = month,
            GrossSalary = grossSalary,
            Deductions = deductions,
            NetSalary = netSalary,
            CreatedBy = createdBy
        };
        return await mediator.Send(command);
    }

    public async Task<ProcessMonthlySalaryResult> ProcessMonthlySalary(
        [Service] IMediator mediator,
        string monthYear,
        long processedBy)
    {
        var command = new ProcessMonthlySalaryCommand
        {
            MonthYear = monthYear,
            ProcessedBy = processedBy
        };
        return await mediator.Send(command);
    }
}
