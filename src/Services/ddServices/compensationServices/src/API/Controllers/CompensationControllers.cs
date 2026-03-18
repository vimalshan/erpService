namespace CompensationService.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using MediatR;
using CompensationService.Application.Commands;
using CompensationService.Application.Queries;
using CompensationService.Application.DTOs;

/// <summary>
/// Controller for budget management operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<BudgetsController> _logger;

    public BudgetsController(IMediator mediator, ILogger<BudgetsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets a budget by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BudgetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBudgetById(decimal id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBudgetByIdQuery(id), cancellationToken);
        if (!result.Success)
            return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Gets budgets for a specific year and business.
    /// </summary>
    [HttpGet("year/{yearId}/business/{businessId}")]
    [ProducesResponseType(typeof(ApiResponse<List<BudgetDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBudgetsByYearAndBusiness(decimal yearId, decimal businessId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBudgetsByYearAndBusinessQuery(yearId, businessId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new budget.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BudgetDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBudget([FromBody] CreateUpdateBudgetDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateBudgetCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetBudgetById), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Updates an existing budget.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BudgetDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBudget(decimal id, [FromBody] CreateUpdateBudgetDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateBudgetCommand(id, dto);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}

/// <summary>
/// Controller for compensation level management operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CompensationLevelsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CompensationLevelsController> _logger;

    public CompensationLevelsController(IMediator mediator, ILogger<CompensationLevelsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets a compensation level by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CompensationLevelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLevelById(decimal id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCompensationLevelByIdQuery(id), cancellationToken);
        if (!result.Success)
            return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Gets all active compensation levels.
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<List<CompensationLevelDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveLevels(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveLevelsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all compensation levels.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<CompensationLevelDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllLevels(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllLevelsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new compensation level.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CompensationLevelDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLevel([FromBody] CreateUpdateCompensationLevelDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateCompensationLevelCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetLevelById), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Closes a compensation level.
    /// </summary>
    [HttpPost("{id}/close")]
    [ProducesResponseType(typeof(ApiResponse<CompensationLevelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CloseLevel(decimal id, CancellationToken cancellationToken)
    {
        var command = new CloseCompensationLevelCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}

/// <summary>
/// Controller for compensation period management operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CompensationPeriodsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CompensationPeriodsController> _logger;

    public CompensationPeriodsController(IMediator mediator, ILogger<CompensationPeriodsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets a period by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CompensationPeriodDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPeriodById(decimal id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCompensationPeriodByIdQuery(id), cancellationToken);
        if (!result.Success)
            return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Gets periods for a specific year.
    /// </summary>
    [HttpGet("year/{yearId}")]
    [ProducesResponseType(typeof(ApiResponse<List<CompensationPeriodDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPeriodsByYear(decimal yearId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPeriodsByYearQuery(yearId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all open periods.
    /// </summary>
    [HttpGet("open")]
    [ProducesResponseType(typeof(ApiResponse<List<CompensationPeriodDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpenPeriods(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOpenPeriodsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new period.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CompensationPeriodDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePeriod([FromBody] CreateUpdateCompensationPeriodDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateCompensationPeriodCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetPeriodById), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Generates a circular for a period.
    /// </summary>
    [HttpPost("{id}/generate-circular")]
    [ProducesResponseType(typeof(ApiResponse<CompensationPeriodDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateCircular(decimal id, CancellationToken cancellationToken)
    {
        var command = new GenerateCircularCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Confirms a period to payroll.
    /// </summary>
    [HttpPost("{id}/confirm-payroll")]
    [ProducesResponseType(typeof(ApiResponse<CompensationPeriodDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmToPayroll(decimal id, CancellationToken cancellationToken)
    {
        var command = new ConfirmPeriodToPayrollCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}

/// <summary>
/// Controller for compensation recommendation management operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CompensationRecommendationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CompensationRecommendationsController> _logger;

    public CompensationRecommendationsController(IMediator mediator, ILogger<CompensationRecommendationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets a recommendation by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CompensationRecommendationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecommendationById(decimal id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCompensationRecommendationByIdQuery(id), cancellationToken);
        if (!result.Success)
            return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Gets all recommendations for a period with pagination.
    /// </summary>
    [HttpGet("period/{periodId}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResultDto<CompensationRecommendationDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecommendationsByPeriod(decimal periodId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRecommendationsByPeriodQuery(periodId, pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new recommendation.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CompensationRecommendationDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRecommendation([FromBody] CreateCompensationRecommendationDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateCompensationRecommendationCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetRecommendationById), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Submits a recommendation.
    /// </summary>
    [HttpPost("submit")]
    [ProducesResponseType(typeof(ApiResponse<CompensationRecommendationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitRecommendation([FromBody] SubmitRecommendationDto dto, CancellationToken cancellationToken)
    {
        var command = new SubmitRecommendationCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Approves a recommendation.
    /// </summary>
    [HttpPost("approve")]
    [ProducesResponseType(typeof(ApiResponse<CompensationRecommendationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApproveRecommendation([FromBody] ApproveRecommendationDto dto, CancellationToken cancellationToken)
    {
        var command = new ApproveRecommendationCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Rejects a recommendation.
    /// </summary>
    [HttpPost("reject")]
    [ProducesResponseType(typeof(ApiResponse<CompensationRecommendationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RejectRecommendation([FromBody] RejectRecommendationDto dto, CancellationToken cancellationToken)
    {
        var command = new RejectRecommendationCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
