using EmployeeService.Application.DTOs;
using EmployeeService.Application.Queries;
using MediatR;

namespace EmployeeService.API.GraphQL;

/// <summary>
/// GraphQL Query root type for Employee operations
/// </summary>
public class EmployeeQuery
{
    /// <summary>Get all employees</summary>
    public async Task<List<EmployeeDto>> GetEmployees([Service] IMediator mediator)
    {
        var result = await mediator.Send(new GetAllEmployeesQuery());
        return result.ToList();
    }

    /// <summary>Get employee by ID</summary>
    public async Task<EmployeeDto?> GetEmployeeById(
        [Service] IMediator mediator,
        long id)
    {
        return await mediator.Send(new GetEmployeeByIdQuery(id));
    }

    /// <summary>Get employees by cost center</summary>
    public async Task<List<EmployeeDto>> GetEmployeesByCostCenter(
        [Service] IMediator mediator,
        string costCenterId)
    {
        var result = await mediator.Send(new GetEmployeesByCostCenterQuery { CostCenterId = costCenterId });
        return result.ToList();
    }

    /// <summary>Search employees by name or email</summary>
    public async Task<List<EmployeeDto>> SearchEmployees(
        [Service] IMediator mediator,
        string searchTerm)
    {
        var result = await mediator.Send(new SearchEmployeesQuery { SearchTerm = searchTerm });
        return result.ToList();
    }

    /// <summary>Get salary increment logs for employee</summary>
    public async Task<List<SalaryIncrementLogDto>> GetSalaryIncrementLogs(
        [Service] IMediator mediator,
        long employeeSystemId)
    {
        var result = await mediator.Send(new GetSalaryIncrementLogsQuery { EmployeeSystemId = employeeSystemId });
        return result.ToList();
    }

    /// <summary>Get salary increments by date range</summary>
    public async Task<List<SalaryIncrementLogDto>> GetSalaryIncrementsByDateRange(
        [Service] IMediator mediator,
        DateTime startDate,
        DateTime endDate)
    {
        var result = await mediator.Send(new GetSalaryIncrementLogsByDateRangeQuery
        {
            StartDate = startDate,
            EndDate = endDate
        });
        return result.ToList();
    }

    /// <summary>Get CTC history for employee</summary>
    public async Task<List<SalaryIncrementLogDto>> GetEmployeeCTCHistory(
        [Service] IMediator mediator,
        long employeeSystemId)
    {
        var result = await mediator.Send(new GetEmployeeCTCHistoryQuery { EmployeeSystemId = employeeSystemId });
        return result.ToList();
    }
}
