namespace CheckupManagementService.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CheckupManagementService.Application.Commands;
using CheckupManagementService.Application.Queries;
using CheckupManagementService.DTOs;
using Shared.Infrastructure.Authentication;

/// <summary>
/// Checkup Management Controller
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class CheckupsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;
    private readonly ILogger<CheckupsController> _logger;

    public CheckupsController(
        IMediator mediator,
        IUserContext userContext,
        ILogger<CheckupsController> logger)
    {
        _mediator = mediator;
        _userContext = userContext;
        _logger = logger;
    }

    /// <summary>
    /// Get all checkups with pagination and filtering
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetCheckupsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCheckups(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        [FromQuery] string? employeeNumber = null,
        [FromQuery] string? checkupType = null,
        CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Getting checkups: Page {PageNumber}, Size {PageSize}",
            pageNumber,
            pageSize);

        var query = new GetCheckupsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Status = status,
            EmployeeNumber = employeeNumber,
            CheckupType = checkupType
        };

        try
        {
            var result = await _mediator.Send(query, ct);

            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());
            Response.Headers.Add("X-Page-Number", pageNumber.ToString());
            Response.Headers.Add("X-Page-Size", pageSize.ToString());

            return Ok(new { success = true, data = result, statusCode = 200 });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting checkups");
            return BadRequest(new { success = false, message = ex.Message, statusCode = 400 });
        }
    }

    /// <summary>
    /// Get checkup by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CheckupMasterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCheckup(
        [FromRoute] string id,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Getting checkup: {CheckupMasterId}", id);

        var query = new GetCheckupByIdQuery { CheckupMasterId = id };
        var result = await _mediator.Send(query, ct);

        if (result == null)
            return NotFound(new { success = false, message = "Checkup not found", statusCode = 404 });

        return Ok(new { success = true, data = result, statusCode = 200 });
    }

    /// <summary>
    /// Create new checkup
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateCheckupResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCheckup(
        [FromBody] CreateCheckupDto dto,
        CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Creating checkup for employee: {EmployeeNumber}",
            dto.EmployeeNumber);

        try
        {
            var command = new CreateCheckupCommand
            {
                EmployeeNumber = dto.EmployeeNumber,
                CheckupType = dto.CheckupType,
                CheckupDate = dto.CheckupDate,
                DoctorCode = dto.DoctorCode,
                TestIds = dto.TestIds
            };

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(GetCheckup),
                new { id = result.CheckupMasterId },
                new { success = true, data = result, statusCode = 201 });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating checkup");
            return BadRequest(new { success = false, message = ex.Message, statusCode = 400 });
        }
    }

    /// <summary>
    /// Update checkup status
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(UpdateCheckupResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCheckupStatus(
        [FromRoute] string id,
        [FromBody] UpdateCheckupDto dto,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Updating checkup status: {CheckupMasterId}", id);

        try
        {
            var command = new UpdateCheckupStatusCommand
            {
                CheckupMasterId = id,
                Status = dto.Status,
                DoctorRemarks = dto.DoctorRemarks,
                ApprovedBy = _userContext.UserId
            };

            var result = await _mediator.Send(command, ct);

            return Ok(new { success = true, data = result, statusCode = 200 });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Checkup not found: {CheckupMasterId}", id);
            return NotFound(new { success = false, message = ex.Message, statusCode = 404 });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating checkup");
            return BadRequest(new { success = false, message = ex.Message, statusCode = 400 });
        }
    }

    /// <summary>
    /// Get checkups by employee
    /// </summary>
    [HttpGet("employee/{employeeNumber}")]
    [ProducesResponseType(typeof(GetCheckupsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCheckupsByEmployee(
        [FromRoute] string employeeNumber,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Getting checkups for employee: {EmployeeNumber}",
            employeeNumber);

        var query = new GetCheckupsByEmployeeQuery
        {
            EmployeeNumber = employeeNumber,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query, ct);

        Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());

        return Ok(new { success = true, data = result, statusCode = 200 });
    }
}

/// <summary>
/// Health Examination Controller
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class HealthExaminationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<HealthExaminationsController> _logger;

    public HealthExaminationsController(
        IMediator mediator,
        ILogger<HealthExaminationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get health examination by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HealthMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHealthExamination(
        [FromRoute] string id,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Getting health examination: {HealthId}", id);

        var query = new GetHealthExaminationQuery { HealthId = id };
        var result = await _mediator.Send(query, ct);

        if (result == null)
            return NotFound(new { success = false, message = "Health examination not found", statusCode = 404 });

        return Ok(new { success = true, data = result, statusCode = 200 });
    }

    /// <summary>
    /// Record health examination
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(RecordHealthExaminationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RecordHealthExamination(
        [FromBody] CreateHealthExaminationDto dto,
        CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Recording health examination for checkup: {CheckupMasterId}",
            dto.CheckupMasterId);

        try
        {
            var command = new RecordHealthExaminationCommand
            {
                CheckupMasterId = dto.CheckupMasterId,
                EmployeeNumber = dto.EmployeeNumber,
                Height = dto.Height,
                Weight = dto.Weight,
                BloodPressure = dto.BloodPressure,
                HeartRate = dto.HeartRate,
                BloodGroup = dto.BloodGroup,
                EyeVision = dto.EyeVision,
                TestResults = dto.TestResults.Select(tr => new HealthTestResultInput
                {
                    TestName = tr.TestName,
                    TestValue = tr.TestValue,
                    Result = tr.Result,
                    Remarks = tr.Remarks
                }).ToList()
            };

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(GetHealthExamination),
                new { id = result.HealthId },
                new { success = true, data = result, statusCode = 201 });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording health examination");
            return BadRequest(new { success = false, message = ex.Message, statusCode = 400 });
        }
    }
}

/// <summary>
/// Test Master Controller
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class TestMastersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TestMastersController> _logger;

    public TestMastersController(
        IMediator mediator,
        ILogger<TestMastersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all tests with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetTestMastersResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestMasters(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? category = null,
        CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Getting test masters: Page {PageNumber}, Size {PageSize}",
            pageNumber,
            pageSize);

        var query = new GetTestMastersQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            IsActive = isActive,
            Category = category
        };

        var result = await _mediator.Send(query, ct);

        Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());

        return Ok(new { success = true, data = result, statusCode = 200 });
    }

    /// <summary>
    /// Create new test master
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(CreateTestMasterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTestMaster(
        [FromBody] CreateTestMasterDto dto,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Creating test master: {TestName}", dto.TestName);

        try
        {
            var command = new CreateTestMasterCommand
            {
                TestName = dto.TestName,
                TestCategory = dto.TestCategory,
                NormalRange = dto.NormalRange,
                Unit = dto.Unit,
                Cost = dto.Cost
            };

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(CreateTestMaster),
                new { },
                new { success = true, data = result, statusCode = 201 });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating test master");
            return BadRequest(new { success = false, message = ex.Message, statusCode = 400 });
        }
    }
}

/// <summary>
/// Checkup Others Controller
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class CheckupOthersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CheckupOthersController> _logger;

    public CheckupOthersController(
        IMediator mediator,
        ILogger<CheckupOthersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Record checkup other details
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(RecordCheckupOthersResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RecordCheckupOthers(
        [FromBody] CreateCheckupOthersDto dto,
        CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Recording checkup others for checkup: {CheckupMasterId}",
            dto.CheckupMasterId);

        try
        {
            var command = new RecordCheckupOthersCommand
            {
                CheckupMasterId = dto.CheckupMasterId,
                MedicineAllergy = dto.MedicineAllergy,
                FamilyHistory = dto.FamilyHistory,
                PastSurgery = dto.PastSurgery,
                CurrentMedicines = dto.CurrentMedicines,
                LifestyleHabits = dto.LifestyleHabits,
                OtherComments = dto.OtherComments
            };

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(RecordCheckupOthers),
                new { },
                new { success = true, data = result, statusCode = 201 });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording checkup others");
            return BadRequest(new { success = false, message = ex.Message, statusCode = 400 });
        }
    }
}

/// <summary>
/// Health Check Card Controller
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class HealthCheckCardsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<HealthCheckCardsController> _logger;

    public HealthCheckCardsController(
        IMediator mediator,
        ILogger<HealthCheckCardsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Issue health check card
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(IssueHealthCheckCardResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> IssueHealthCheckCard(
        [FromBody] IssueHealthCheckCardDto dto,
        CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Issuing health check card for checkup: {CheckupMasterId}",
            dto.CheckupMasterId);

        try
        {
            var command = new IssueHealthCheckCardCommand
            {
                CheckupMasterId = dto.CheckupMasterId,
                EmployeeNumber = dto.EmployeeNumber,
                ExpiryDate = dto.ExpiryDate,
                IssuedBy = dto.IssuedBy
            };

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(IssueHealthCheckCard),
                new { },
                new { success = true, data = result, statusCode = 201 });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error issuing health check card");
            return BadRequest(new { success = false, message = ex.Message, statusCode = 400 });
        }
    }
}

/// <summary>
/// Reports Controller
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(
        IMediator mediator,
        ILogger<ReportsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get checkup status report
    /// </summary>
    [HttpGet("checkup-status")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(CheckupStatusReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCheckupStatusReport(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Generating checkup status report");

        var query = new GetCheckupStatusReportQuery
        {
            FromDate = fromDate,
            ToDate = toDate
        };

        var result = await _mediator.Send(query, ct);

        return Ok(new { success = true, data = result, statusCode = 200 });
    }
}

// DTO for IssueHealthCheckCard
public class IssueHealthCheckCardDto
{
    public string CheckupMasterId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public string? IssuedBy { get; set; }
}
