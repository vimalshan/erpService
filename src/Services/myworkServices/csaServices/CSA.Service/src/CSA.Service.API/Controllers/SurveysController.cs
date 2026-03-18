using CSA.Service.Application.Commands.Surveys;
using CSA.Service.Application.DTOs;
using CSA.Service.Application.Queries.Surveys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSA.Service.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SurveysController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SurveyDto>>> GetAll(CancellationToken ct) =>
        Ok(await mediator.Send(new GetAllSurveysQuery(), ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SurveyDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSurveyByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:long}/questions")]
    public async Task<ActionResult<IEnumerable<SurveyQuestionDto>>> GetQuestions(long id, CancellationToken ct) =>
        Ok(await mediator.Send(new GetSurveyQuestionsQuery(id), ct));

    [HttpGet("questions/{questionId:long}/feedbacks")]
    public async Task<ActionResult<IEnumerable<SurveyFeedbackDto>>> GetFeedbacks(long questionId, CancellationToken ct) =>
        Ok(await mediator.Send(new GetSurveyFeedbacksQuery(questionId), ct));

    [HttpPost]
    public async Task<ActionResult<SurveyDto>> Create([FromBody] CreateSurveyDto dto, CancellationToken ct)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new CreateSurveyCommand(dto, userId), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.SurveyId }, result);
    }

    [HttpPost("questions")]
    public async Task<ActionResult<SurveyQuestionDto>> CreateQuestion([FromBody] CreateSurveyQuestionCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Created($"/api/surveys/{result.SurveyId}/questions", result);
    }

    [HttpPost("feedback")]
    public async Task<ActionResult<SurveyFeedbackDto>> SubmitFeedback([FromBody] SubmitSurveyFeedbackCommand command, CancellationToken ct) =>
        Ok(await mediator.Send(command, ct));

    [HttpPost("feedback/approve")]
    public async Task<ActionResult<SurveyFeedbackDto>> ApproveFeedback([FromBody] ApproveSurveyFeedbackCommand command, CancellationToken ct) =>
        Ok(await mediator.Send(command, ct));

    private long GetUserId()
    {
        var claim = User.FindFirst("sub") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim != null && long.TryParse(claim.Value, out var id) ? id : 0;
    }
}
