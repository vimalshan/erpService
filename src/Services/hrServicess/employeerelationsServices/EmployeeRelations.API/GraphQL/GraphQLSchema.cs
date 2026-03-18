using MediatR;
using EmployeeRelations.Application.DTOs;
using EmployeeRelations.Application.Commands.Disciplinary;
using EmployeeRelations.Application.Commands.Ews;
using EmployeeRelations.Application.Commands.Survey;
using EmployeeRelations.Application.Queries.Disciplinary;
using EmployeeRelations.Application.Queries.Ews;
using EmployeeRelations.Application.Queries.Survey;
using HotChocolate.Authorization;

namespace EmployeeRelations.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<DisciplinaryMainDto>> GetDisciplinaryCasesAsync(
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllDisciplinaryCasesQuery(), ct);

    public async Task<DisciplinaryMainDto> GetDisciplinaryCaseAsync(long id,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetDisciplinaryCaseQuery(id), ct);

    public async Task<EwsMainDto> GetEwsAsync(long id,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetEwsByIdQuery(id), ct);

    public async Task<IEnumerable<EwsMainDto>> GetEwsByEmployeeAsync(long empSysId,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetEwsByEmpQuery(empSysId), ct);

    public async Task<IEnumerable<EwsMainDto>> GetEwsByPeriodAsync(int periodNo,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetEwsByPeriodQuery(periodNo), ct);

    public async Task<IEnumerable<SurveyMasterDto>> GetSurveysAsync(
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetAllSurveysQuery(), ct);

    public async Task<SurveyMasterDto> GetSurveyAsync(long id,
        [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(new GetSurveyByIdQuery(id), ct);
}

public class Mutation
{
    public async Task<DisciplinaryMainDto> CreateDisciplinaryCaseAsync(
        CreateDisciplinaryCaseCommand input, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<EwsMainDto> CreateEwsAsync(
        CreateEwsCommand input, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<SurveyMasterDto> CreateSurveyAsync(
        CreateSurveyCommand input, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<SurveyResponseDto> SubmitSurveyResponseAsync(
        SubmitSurveyResponseCommand input, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);
}
