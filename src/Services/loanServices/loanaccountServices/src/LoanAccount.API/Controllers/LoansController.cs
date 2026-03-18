using LoanAccount.Application.Commands;
using LoanAccount.Application.Queries;
using LoanAccount.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoanAccount.API.Controllers;

/// <summary>
/// REST API controller for loan operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LoansController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoansController> _logger;

    public LoansController(IMediator mediator, ILogger<LoansController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Create a new loan
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<long>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLoan([FromBody] CreateLoanRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating loan for employee {EmployeeId}", request.EmployeeId);

        var command = new CreateLoanCommand(
            request.LoanAppId,
            request.EmployeeId,
            request.LoanId,
            request.GradeId,
            request.PrincipalAmount,
            request.DisbursementType,
            request.LoanDate,
            request.FirstInstallmentDate,
            request.UnitId,
            request.SubClassId,
            request.Reason,
            request.GuarantorId,
            GetCurrentUserId());

        var loanNo = await _mediator.Send(command, cancellationToken);

        _logger.LogInformation("Loan created successfully with LoanNo: {LoanNo}", loanNo);
        return CreatedAtAction(nameof(GetLoanByNumber), new { loanNo }, new ApiResponse<long> { Data = loanNo });
    }

    /// <summary>
    /// Get loan by loan number
    /// </summary>
    [HttpGet("{loanNo}")]
    [ProducesResponseType(typeof(ApiResponse<LoanResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLoanByNumber(long loanNo, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching loan details for LoanNo: {LoanNo}", loanNo);

        var query = new GetLoanByNumberQuery(loanNo);
        var loan = await _mediator.Send(query, cancellationToken);

        if (loan is null)
            return NotFound(new ApiErrorResponse { Message = $"Loan {loanNo} not found" });

        return Ok(new ApiResponse<LoanResponse> { Data = loan });
    }

    /// <summary>
    /// Get all loans for an employee
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LoanResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeLoans(long employeeId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching loans for employee {EmployeeId}", employeeId);

        var query = new GetEmployeeLoansQuery(employeeId);
        var loans = await _mediator.Send(query, cancellationToken);

        return Ok(new ApiResponse<IEnumerable<LoanResponse>> { Data = loans });
    }

    /// <summary>
    /// Get loan details with installments and ledger
    /// </summary>
    [HttpGet("{loanNo}/details")]
    [ProducesResponseType(typeof(ApiResponse<LoanDetailsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLoanDetails(long loanNo, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching detailed loan information for LoanNo: {LoanNo}", loanNo);

        var query = new GetLoanDetailsQuery(loanNo);
        var details = await _mediator.Send(query, cancellationToken);

        if (details is null)
            return NotFound(new ApiErrorResponse { Message = $"Loan {loanNo} not found" });

        return Ok(new ApiResponse<LoanDetailsResponse> { Data = details });
    }

    /// <summary>
    /// Approve a loan
    /// </summary>
    [HttpPost("{loanNo}/approve")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApproveLoan(long loanNo, [FromBody] ApproveLoanRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Approving loan {LoanNo} with interest rate {InterestRate}", loanNo, request.InterestRate);

        var command = new ApproveLoanCommand(
            loanNo,
            request.InterestRate,
            GetCurrentUserId(),
            request.ApprovalRemarks);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new ApiResponse<bool> { Data = result });
    }

    /// <summary>
    /// Disburse a loan
    /// </summary>
    [HttpPost("{loanNo}/disburse")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DisburseLoan(long loanNo, [FromQuery] decimal amount, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Disbursing loan {LoanNo} with amount {Amount}", loanNo, amount);

        var command = new DisburseLoanCommand(loanNo, amount, GetCurrentUserId());
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse<bool> { Data = result });
    }

    /// <summary>
    /// Record EMI payment
    /// </summary>
    [HttpPost("{loanNo}/payment")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RecordEMIPayment(long loanNo, [FromBody] RecordEMIPaymentRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Recording EMI payment for loan {LoanNo}", loanNo);

        var command = new RecordEMIPaymentCommand(
            request.InstallmentId,
            loanNo,
            request.PrincipalPaid,
            request.InterestPaid,
            request.PaymentDate,
            GetCurrentUserId());

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new ApiResponse<bool> { Data = result });
    }

    /// <summary>
    /// Get loan installments
    /// </summary>
    [HttpGet("{loanNo}/installments")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<InstallmentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInstallments(long loanNo, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching installments for loan {LoanNo}", loanNo);

        var query = new GetLoanInstallmentsQuery(loanNo);
        var installments = await _mediator.Send(query, cancellationToken);

        return Ok(new ApiResponse<IEnumerable<InstallmentResponse>> { Data = installments });
    }

    /// <summary>
    /// Get active loans
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LoanResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveLoans(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all active loans");

        var query = new GetActiveLoansQuery();
        var loans = await _mediator.Send(query, cancellationToken);

        return Ok(new ApiResponse<IEnumerable<LoanResponse>> { Data = loans });
    }

    /// <summary>
    /// Settle a loan
    /// </summary>
    [HttpPost("{loanNo}/settle")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SettleLoan(long loanNo, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Settling loan {LoanNo}", loanNo);

        var command = new SettleLoanCommand(loanNo, GetCurrentUserId());
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse<bool> { Data = result });
    }

    private long GetCurrentUserId()
    {
        // In production, this would extract from claims
        return User.FindFirst("uid")?.Value is { } id && long.TryParse(id, out var userId)
            ? userId
            : 1; // Default for demo
    }
}

/// <summary>
/// API response wrapper
/// </summary>
public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = "Operation successful";
}

/// <summary>
/// API error response wrapper
/// </summary>
public class ApiErrorResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string>? Errors { get; set; }
}
