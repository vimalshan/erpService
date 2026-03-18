using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using HRService.Application.Commands;
using HRService.Application.Queries;
using HRService.Application.DTOs;

namespace HRService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IMediator mediator, ILogger<EmployeesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateEmployeeCommand
            {
                EmployeeCode = dto.EmployeeCode,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DepartmentId = dto.DepartmentId,
                PositionId = dto.PositionId,
                SiteId = dto.SiteId,
                JoinDate = dto.JoinDate,
                EmploymentType = dto.EmploymentType,
                ManagerId = dto.ManagerId
            };

            var employeeId = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation("Employee created with ID: {EmployeeId}", employeeId);

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, new { employeeId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployeeById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetEmployeeByIdQuery(id);
            var employee = await _mediator.Send(query, cancellationToken);
            return employee == null ? NotFound() : Ok(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAllEmployeesQuery { PageNumber = pageNumber, PageSize = pageSize };
            var employees = await _mediator.Send(query, cancellationToken);
            return Ok(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/terminate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TerminateEmployee(Guid id, [FromBody] TerminateEmployeeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var command = new TerminateEmployeeCommand
            {
                EmployeeId = id,
                TerminationDate = dto.TerminationDate,
                Reason = dto.Reason
            };

            var result = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation("Employee {EmployeeId} terminated", id);

            return result ? Ok(new { message = "Employee terminated successfully" }) : BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error terminating employee");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/suspend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SuspendEmployee(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new SuspendEmployeeCommand { EmployeeId = id };
            var result = await _mediator.Send(command, cancellationToken);
            return result ? Ok(new { message = "Employee suspended" }) : BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error suspending employee");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/resume")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ResumeEmployee(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new ResumeEmployeeCommand { EmployeeId = id };
            var result = await _mediator.Send(command, cancellationToken);
            return result ? Ok(new { message = "Employee resumed" }) : BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resuming employee");
            return BadRequest(new { message = ex.Message });
        }
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeavesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LeavesController> _logger;

    public LeavesController(IMediator mediator, ILogger<LeavesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> RequestLeave([FromBody] RequestLeaveDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var command = new RequestLeaveCommand
            {
                EmployeeId = dto.EmployeeId,
                LeaveTypeId = dto.LeaveTypeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason
            };

            var leaveId = await _mediator.Send(command, cancellationToken);
            _logger.LogInformation("Leave requested with ID: {LeaveId}", leaveId);

            return CreatedAtAction(nameof(GetLeave), new { id = leaveId }, new { leaveId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting leave");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeave(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetEmployeeLeaveQuery(id);
            var leave = await _mediator.Send(query, cancellationToken);
            return leave == null ? NotFound() : Ok(leave);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving leave");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> ApproveLeave(Guid id, [FromBody] ApproveLeaveDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var command = new ApproveLeaveCommand { LeaveId = id, ApprovedBy = dto.ApprovedBy };
            var result = await _mediator.Send(command, cancellationToken);
            return result ? Ok(new { message = "Leave approved" }) : BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving leave");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> RejectLeave(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new RejectLeaveCommand { LeaveId = id };
            var result = await _mediator.Send(command, cancellationToken);
            return result ? Ok(new { message = "Leave rejected" }) : BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting leave");
            return BadRequest(new { message = ex.Message });
        }
    }
}
