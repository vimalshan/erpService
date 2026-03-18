using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeRelations.Application.Commands.Survey;
using EmployeeRelations.Application.Queries.Survey;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Infrastructure.Services;

namespace EmployeeRelations.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class SurveyController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBlobStorageService _blobStorage;
    public SurveyController(IMediator mediator, IBlobStorageService blobStorage)
    {
        _mediator = mediator;
        _blobStorage = blobStorage;
    }

    /// <summary>Get all surveys.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SurveyMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _mediator.Send(new GetAllSurveysQuery(), ct));

    /// <summary>Get survey by Id.</summary>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(SurveyMasterDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct) =>
        Ok(await _mediator.Send(new GetSurveyByIdQuery(id), ct));

    /// <summary>Create a new survey.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(SurveyMasterDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateSurveyCommand cmd, CancellationToken ct)
    {
        var result = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Submit a survey response.</summary>
    [HttpPost("{surveyId:long}/responses")]
    [ProducesResponseType(typeof(SurveyResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> SubmitResponse(long surveyId, [FromBody] SubmitSurveyResponseCommand cmd, CancellationToken ct)
    {
        var result = await _mediator.Send(cmd with { SurveyId = surveyId }, ct);
        return CreatedAtAction(nameof(GetById), new { id = surveyId }, result);
    }

    /// <summary>Upload survey image to blob storage.</summary>
    [HttpPost("{surveyId:long}/image")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadImage(long surveyId, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("No file uploaded.");
        using var stream = file.OpenReadStream();
        var blobName = await _blobStorage.UploadDocumentAsync(stream, file.FileName, "survey-images", ct);
        return Ok(new { BlobName = blobName, SurveyId = surveyId });
    }
}
