using EmployeeService.Application.Commands;
using EmployeeService.Application.DTOs;
using HotChocolate.Authorization;
using MediatR;

namespace EmployeeService.API.GraphQL;

/// <summary>
/// GraphQL Mutation root type for Employee operations
/// </summary>
public class EmployeeMutation
{
    /// <summary>Create a new employee</summary>
    [Authorize(Policy = "AdminOnly")]
    public async Task<EmployeeDto> CreateEmployee(
        [Service] IMediator mediator,
        string firstName,
        string lastName,
        string email,
        string employeeCode,
        DateTime joiningDate,
        string? middleName = null,
        string? phoneNumber = null,
        string? costCenterId = null)
    {
        var command = new CreateEmployeeCommand
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Email = email,
            PhoneNumber = phoneNumber,
            EmployeeCode = employeeCode,
            CostCenterId = costCenterId,
            JoiningDate = joiningDate
        };

        return await mediator.Send(command);
    }

    /// <summary>Update employee information</summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    public async Task<EmployeeDto> UpdateEmployee(
        [Service] IMediator mediator,
        long employeeSystemId,
        string firstName,
        string lastName,
        string email,
        string? phoneNumber = null,
        string? costCenterId = null)
    {
        var command = new UpdateEmployeeCommand
        {
            EmployeeSystemId = employeeSystemId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            CostCenterId = costCenterId
        };

        return await mediator.Send(command);
    }

    /// <summary>Process salary increment</summary>
    [Authorize(Policy = "ManagerOrAdmin")]
    public async Task<SalaryIncrementLogDto> ProcessSalaryIncrement(
        [Service] IMediator mediator,
        long employeeSystemId,
        decimal incrementPercentage,
        DateTime effectiveDate,
        long approvedBy)
    {
        var command = new ProcessSalaryIncrementCommand
        {
            EmployeeSystemId = employeeSystemId,
            IncrementPercentage = incrementPercentage,
            EffectiveDate = effectiveDate,
            ApprovedBy = approvedBy
        };

        return await mediator.Send(command);
    }

    /// <summary>Modify employee CTC</summary>
    [Authorize(Policy = "AdminOnly")]
    public async Task<EmployeeDto> ModifyEmployeeCTC(
        [Service] IMediator mediator,
        long employeeSystemId,
        decimal newGrossCTC,
        decimal newBasicSalary,
        DateTime effectiveDate,
        string reason,
        long modifiedBy)
    {
        var command = new ModifyEmployeeCTCCommand
        {
            EmployeeSystemId = employeeSystemId,
            NewGrossCTC = newGrossCTC,
            NewBasicSalary = newBasicSalary,
            EffectiveDate = effectiveDate,
            Reason = reason,
            ModifiedBy = modifiedBy
        };

        return await mediator.Send(command);
    }

    /// <summary>Terminate employee</summary>
    [Authorize(Policy = "AdminOnly")]
    public async Task<EmployeeDto> TerminateEmployee(
        [Service] IMediator mediator,
        long employeeSystemId,
        DateTime terminationDate)
    {
        var command = new TerminateEmployeeCommand
        {
            EmployeeSystemId = employeeSystemId,
            TerminationDate = terminationDate
        };

        return await mediator.Send(command);
    }

    /// <summary>Delete employee</summary>
    [Authorize(Policy = "AdminOnly")]
    public async Task<bool> DeleteEmployee(
        [Service] IMediator mediator,
        long employeeSystemId)
    {
        await mediator.Send(new DeleteEmployeeCommand { EmployeeSystemId = employeeSystemId });
        return true;
    }
}
