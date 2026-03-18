using CSA.Service.Application.DTOs;
using CSA.Service.Application.Queries.Controls;
using CSA.Service.Application.Queries.Surveys;
using CSA.Service.Application.Queries.Processes;
using MediatR;

namespace CSA.Service.API.GraphQL;

public class CsaQuery
{
    public async Task<IEnumerable<ControlDto>> GetControls([Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllControlsQuery(), ct);

    public async Task<ControlDto?> GetControlById([Service] IMediator mediator, long controlId, CancellationToken ct) =>
        await mediator.Send(new GetControlByIdQuery(controlId), ct);

    public async Task<IEnumerable<ControlDto>> GetControlsByProcess([Service] IMediator mediator, long processId, CancellationToken ct) =>
        await mediator.Send(new GetControlsByProcessQuery(processId), ct);

    public async Task<IEnumerable<EvidenceDto>> GetEvidencesByControl([Service] IMediator mediator, long controlId, CancellationToken ct) =>
        await mediator.Send(new GetEvidencesByControlQuery(controlId), ct);

    public async Task<IEnumerable<SurveyDto>> GetSurveys([Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllSurveysQuery(), ct);

    public async Task<SurveyDto?> GetSurveyById([Service] IMediator mediator, long surveyId, CancellationToken ct) =>
        await mediator.Send(new GetSurveyByIdQuery(surveyId), ct);

    public async Task<IEnumerable<SurveyQuestionDto>> GetSurveyQuestions([Service] IMediator mediator, long surveyId, CancellationToken ct) =>
        await mediator.Send(new GetSurveyQuestionsQuery(surveyId), ct);

    public async Task<IEnumerable<ProcessDto>> GetProcesses([Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllProcessesQuery(), ct);

    public async Task<ProcessDto?> GetProcessById([Service] IMediator mediator, long processId, CancellationToken ct) =>
        await mediator.Send(new GetProcessByIdQuery(processId), ct);

    public async Task<IEnumerable<SubProcessDto>> GetSubProcesses([Service] IMediator mediator, long processId, CancellationToken ct) =>
        await mediator.Send(new GetSubProcessesByProcessQuery(processId), ct);
}
