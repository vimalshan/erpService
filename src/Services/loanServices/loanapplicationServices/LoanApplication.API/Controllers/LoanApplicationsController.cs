using Microsoft.AspNetCore.Mvc;
using MediatR;
using LoanApplication.Application.Commands;
using LoanApplication.Application.Queries;
using LoanApplication.Application.DTOs;

namespace LoanApplication.API.Controllers;

/// <summary>
/// API controller for loan applications
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LoanApplicationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoanApplicationsController> _logger;

    public LoanApplicationsController(IMediator mediator, ILogger<LoanApplicationsController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Create a new loan application
    /// </summary>
    /// <param name="dto">Create loan application request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created loan application ID</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<long>> CreateLoanApplication(
        [FromBody] CreateLoanApplicationDto dto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating loan application for employee {EmployeeId}", dto.EmployeeId);

        var command = new CreateLoanApplicationCommand
        {
            EmployeeId = dto.EmployeeId,
            LoanId = dto.LoanId,
            AppliedBy = dto.EmployeeId, // In real scenario, this would be the authenticated user
            Source = dto.Source,
            Amount = dto.Amount,
            Reason = dto.Reason,
            GuarantorId = dto.GuarantorId,
            SecondGuarantorId = dto.SecondGuarantorId,
            TenureMonths = dto.TenureMonths
        };

        var loanApplicationId = await _mediator.Send(command, cancellationToken);

        _logger.LogInformation("Loan application created successfully with ID {LoanApplicationId}", loanApplicationId);

        return CreatedAtAction(nameof(GetLoanApplicationById), new { id = loanApplicationId }, loanApplicationId);
    }

    /// <summary>
    /// Get loan application by ID
    /// </summary>
    /// <param name="id">Loan application ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Loan application details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoanApplicationDto>> GetLoanApplicationById(
        [FromRoute] long id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving loan application {LoanApplicationId}", id);

        var query = new GetLoanApplicationByIdQuery { LoanApplicationId = id };
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            _logger.LogWarning("Loan application {LoanApplicationId} not found", id);
            return NotFound($"Loan application with ID {id} not found");
        }

        return Ok(result);
    }

    /// <summary>
    /// Get all loan applications for an employee
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of loan applications</returns>
    [HttpGet("employee/{employeeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LoanApplicationDto>>> GetLoanApplicationsByEmployee(
        [FromRoute] long employeeId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving loan applications for employee {EmployeeId}", employeeId);

        var query = new GetLoanApplicationsByEmployeeIdQuery { EmployeeId = employeeId };
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get all loan applications
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all loan applications</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LoanApplicationDto>>> GetAllLoanApplications(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all loan applications");

        var query = new GetAllLoanApplicationsQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get pending loan applications
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of pending loan applications</returns>
    [HttpGet("pending")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LoanApplicationDto>>> GetPendingLoanApplications(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving pending loan applications");

        var query = new GetPendingLoanApplicationsQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Submit loan application for approval
    /// </summary>
    /// <param name="id">Loan application ID</param>
    /// <param name="submittedBy">User submitting the application</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status</returns>
    [HttpPost("{id}/submit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitLoanApplication(
        [FromRoute] long id,
        [FromQuery] long submittedBy,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Submitting loan application {LoanApplicationId}", id);

        var command = new SubmitLoanApplicationCommand
        {
            LoanApplicationId = id,
            SubmittedBy = submittedBy
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return NotFound($"Loan application with ID {id} not found");

        _logger.LogInformation("Loan application {LoanApplicationId} submitted successfully", id);
        return Ok("Loan application submitted successfully");
    }

    /// <summary>
    /// Approve loan application
    /// </summary>
    /// <param name="id">Loan application ID</param>
    /// <param name="dto">Approval details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status</returns>
    [HttpPost("{id}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApproveLoanApplication(
        [FromRoute] long id,
        [FromBody] ApproveLoanApplicationDto dto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Approving loan application {LoanApplicationId} by user {ApprovedBy}", id, dto.ApprovedBy);

        var command = new ApproveLoanApplicationCommand
        {
            LoanApplicationId = id,
            ApprovedBy = dto.ApprovedBy,
            Remarks = dto.Remarks
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return NotFound($"Loan application with ID {id} not found");

        _logger.LogInformation("Loan application {LoanApplicationId} approved successfully", id);
        return Ok("Loan application approved successfully");
    }

    /// <summary>
    /// Reject loan application
    /// </summary>
    /// <param name="id">Loan application ID</param>
    /// <param name="dto">Rejection details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status</returns>
    [HttpPost("{id}/reject")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RejectLoanApplication(
        [FromRoute] long id,
        [FromBody] RejectLoanApplicationDto dto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rejecting loan application {LoanApplicationId} by user {RejectedBy}", id, dto.RejectedBy);

        var command = new RejectLoanApplicationCommand
        {
            LoanApplicationId = id,
            RejectedBy = dto.RejectedBy,
            Remarks = dto.Remarks
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return NotFound($"Loan application with ID {id} not found");

        _logger.LogInformation("Loan application {LoanApplicationId} rejected successfully", id);
        return Ok("Loan application rejected successfully");
    }

    /// <summary>
    /// Disburse loan
    /// </summary>
    /// <param name="id">Loan application ID</param>
    /// <param name="dto">Disbursal details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status</returns>
    [HttpPost("{id}/disburse")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DisburseLoan(
        [FromRoute] long id,
        [FromBody] DisburseLoanApplicationDto dto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Disbursing loan application {LoanApplicationId} by user {DisbursingBy}", id, dto.DisbursingBy);

        var command = new DisburseLoanCommand
        {
            LoanApplicationId = id,
            DisbursingBy = dto.DisbursingBy
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return NotFound($"Loan application with ID {id} not found");

        _logger.LogInformation("Loan application {LoanApplicationId} disbursed successfully", id);
        return Ok("Loan amount disbursed successfully");
    }

    /// <summary>
    /// Check loan eligibility
    /// </summary>
    /// <param name="employeeId">Employee ID</param>
    /// <param name="loanTypeId">Loan type ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Eligibility check result</returns>
    [HttpGet("eligibility/{employeeId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<EligibilityCheckDto>> CheckLoanEligibility(
        [FromRoute] long employeeId,
        [FromQuery] long loanTypeId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking loan eligibility for employee {EmployeeId} for loan type {LoanTypeId}", employeeId, loanTypeId);

        var query = new CheckLoanEligibilityQuery
        {
            EmployeeId = employeeId,
            LoanTypeId = loanTypeId
        };

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
