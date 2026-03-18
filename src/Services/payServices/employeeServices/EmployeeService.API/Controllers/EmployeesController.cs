using System.Threading;
using System.Threading.Tasks;
using EmployeeService.Application.Commands;
using EmployeeService.Application.DTOs;
using EmployeeService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.API.Controllers;

/// <summary>
/// Employee management API endpoints
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IMediator mediator, ILogger<EmployeesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "EmployeeAccess")]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEmployees(int pageNumber = 1, int pageSize = 50, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all employees");
        var query = new GetAllEmployeesQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get employee by system ID
    /// </summary>
    [HttpGet("{employeeSystemId}")]
    [Authorize(Policy = "EmployeeAccess")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployee(long employeeSystemId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving employee {EmployeeSystemId}", employeeSystemId);
        var query = new GetEmployeeByIdQuery(employeeSystemId);
        var result = await _mediator.Send(query, cancellationToken);
        
        if (result == null)
            return NotFound(new { message = "Employee not found" });

        return Ok(result);
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new employee {EmployeeCode}", dto.EmployeeCode);
        var command = new CreateEmployeeCommand
        {
            EmployeeSystemId = dto.EmployeeSystemId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            EmployeeCode = dto.EmployeeCode,
            CostCenterId = dto.CostCenterId,
            JoiningDate = dto.JoiningDate,
            GrossCTC = dto.GrossCTC,
            BasicSalary = dto.BasicSalary,
            CTCEffectiveDate = dto.CTCEffectiveDate
        };

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetEmployee), new { employeeSystemId = result.EmployeeSystemId }, result);
    }

    /// <summary>
    /// Update employee information
    /// </summary>
    [HttpPut("{employeeSystemId}")]
    [Authorize(Policy = "ManagerOrAdmin")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEmployee(long employeeSystemId, [FromBody] UpdateEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating employee {EmployeeSystemId}", employeeSystemId);
        var command = new UpdateEmployeeCommand
        {
            EmployeeSystemId = employeeSystemId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            CostCenterId = dto.CostCenterId
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Process salary increment
    /// </summary>
    [HttpPost("{employeeSystemId}/increment")]
    [Authorize(Policy = "ManagerOrAdmin")]
    [ProducesResponseType(typeof(SalaryIncrementLogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessSalaryIncrement(
        long employeeSystemId,
        [FromBody] SalaryIncrementRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing salary increment for employee {EmployeeSystemId}", employeeSystemId);
        var command = new ProcessSalaryIncrementCommand
        {
            EmployeeSystemId = employeeSystemId,
            IncrementPercentage = dto.IncrementPercentage,
            EffectiveDate = dto.EffectiveDate,
            ApprovedBy = long.Parse(User.FindFirst("sub")?.Value ?? "0")
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get salary increment history for employee
    /// </summary>
    [HttpGet("{employeeSystemId}/salary-history")]
    [Authorize(Policy = "EmployeeAccess")]
    [ProducesResponseType(typeof(IReadOnlyList<SalaryIncrementLogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSalaryHistory(long employeeSystemId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving salary history for employee {EmployeeSystemId}", employeeSystemId);
        var query = new GetEmployeeCTCHistoryQuery { EmployeeSystemId = employeeSystemId };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Search employees
    /// </summary>
    [HttpGet("search/find")]
    [Authorize(Policy = "EmployeeAccess")]
    [ProducesResponseType(typeof(IReadOnlyList<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchEmployees([FromQuery] string? searchTerm, [FromQuery] string? employmentStatus, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching employees with term: {SearchTerm}", searchTerm);
        var query = new SearchEmployeesQuery { SearchTerm = searchTerm, EmploymentStatus = employmentStatus };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Delete employee
    /// </summary>
    [HttpDelete("{employeeSystemId}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmployee(long employeeSystemId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting employee {EmployeeSystemId}", employeeSystemId);
        var command = new DeleteEmployeeCommand { EmployeeSystemId = employeeSystemId };
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result)
            return NotFound(new { message = "Employee not found" });

        return NoContent();
    }
}
