using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todos.Application.Commands;
using Todos.Application.DTOs;
using Todos.Application.Queries;

namespace Todos.API.Controllers;

/// <summary>
/// REST API controller for Learning Records
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class LearningRecordsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LearningRecordsController> _logger;

    public LearningRecordsController(IMediator mediator, ILogger<LearningRecordsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets a learning record by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<LearningRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LearningRecordDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LearningRecordDto>>> GetLearningRecord(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting learning record with ID: {RecordId}", id);
        var result = await _mediator.Send(new GetLearningRecordByIdQuery { Id = id }, cancellationToken);

        if (!result.Success || result.Data == null)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets all learning records with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LearningRecordDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<LearningRecordDto>>>> GetAllLearningRecords(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting learning records - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
        var result = await _mediator.Send(new GetAllLearningRecordsQuery { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Searches learning records by request number
    /// </summary>
    [HttpGet("search/{requestNumber}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LearningRecordDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<LearningRecordDto>>>> SearchByRequestNumber(
        decimal requestNumber,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching learning records for request number: {RequestNumber}", requestNumber);
        var result = await _mediator.Send(new SearchLearningRecordsByRequestNumberQuery { RequestNumber = requestNumber }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new learning record
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LearningRecordDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<LearningRecordDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LearningRecordDto>>> CreateLearningRecord(
        [FromBody] CreateLearningRecordDto createDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating learning record for employee: {EmployeeId}", createDto.EmployeeId);

        var command = new CreateLearningRecordCommand
        {
            LetId = createDto.LetId,
            RequestNumber = createDto.RequestNumber,
            EmployeeId = createDto.EmployeeId,
            SpecificNeed = createDto.SpecificNeed,
            ModifiedBy = createDto.ModifiedBy
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetLearningRecord), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Updates a learning record
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<LearningRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LearningRecordDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<LearningRecordDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LearningRecordDto>>> UpdateLearningRecord(
        Guid id,
        [FromBody] UpdateLearningRecordDto updateDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating learning record with ID: {RecordId}", id);

        var command = new UpdateLearningRecordCommand
        {
            Id = id,
            SpecificNeed = updateDto.SpecificNeed,
            Indicator = updateDto.Indicator,
            DevelopmentArea = updateDto.DevelopmentArea,
            ExpectedPostTraining = updateDto.ExpectedPostTraining,
            BhrStatus = updateDto.BhrStatus,
            ModifiedBy = updateDto.ModifiedBy
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a learning record
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteLearningRecord(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting learning record with ID: {RecordId}", id);

        var command = new DeleteLearningRecordCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Identifies a learning need for a record
    /// </summary>
    [HttpPost("{id}/identify-need")]
    [ProducesResponseType(typeof(ApiResponse<LearningRecordDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LearningRecordDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<LearningRecordDto>>> IdentifyLearningNeed(
        Guid id,
        [FromBody] IdentifyLearningNeedRequest request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Identifying learning need for record: {RecordId}", id);

        var command = new IdentifyLearningNeedCommand
        {
            LearningRecordId = id,
            DevelopmentArea = request.DevelopmentArea,
            Indicator = request.Indicator
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}

/// <summary>
/// Request model for identifying learning needs
/// </summary>
public class IdentifyLearningNeedRequest
{
    public string DevelopmentArea { get; set; } = string.Empty;
    public string Indicator { get; set; } = string.Empty;
}
