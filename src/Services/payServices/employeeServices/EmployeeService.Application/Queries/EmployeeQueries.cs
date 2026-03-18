using System;
using System.Collections.Generic;
using EmployeeService.Application.DTOs;
using MediatR;

namespace EmployeeService.Application.Queries;

/// <summary>
/// Query to get employee by system ID
/// </summary>
public class GetEmployeeByIdQuery : IRequest<EmployeeDto?>
{
    public long EmployeeSystemId { get; set; }

    public GetEmployeeByIdQuery(long employeeSystemId)
    {
        EmployeeSystemId = employeeSystemId;
    }
}

/// <summary>
/// Query to get all employees
/// </summary>
public class GetAllEmployeesQuery : IRequest<IReadOnlyList<EmployeeDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

/// <summary>
/// Query to get employees by cost center
/// </summary>
public class GetEmployeesByCostCenterQuery : IRequest<IReadOnlyList<EmployeeDto>>
{
    public string CostCenterId { get; set; } = string.Empty;
}

/// <summary>
/// Query to get salary increment logs
/// </summary>
public class GetSalaryIncrementLogsQuery : IRequest<IReadOnlyList<SalaryIncrementLogDto>>
{
    public long? EmployeeSystemId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

/// <summary>
/// Query to get salary increment logs by date range
/// </summary>
public class GetSalaryIncrementLogsByDateRangeQuery : IRequest<IReadOnlyList<SalaryIncrementLogDto>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

/// <summary>
/// Query to get employee CTC history
/// </summary>
public class GetEmployeeCTCHistoryQuery : IRequest<IReadOnlyList<SalaryIncrementLogDto>>
{
    public long EmployeeSystemId { get; set; }
}

/// <summary>
/// Query to search employees
/// </summary>
public class SearchEmployeesQuery : IRequest<IReadOnlyList<EmployeeDto>>
{
    public string? SearchTerm { get; set; }
    public string? EmploymentStatus { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
