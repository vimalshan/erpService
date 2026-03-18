using CSA.Service.Application.Commands.Controls;
using CSA.Service.Application.Commands.Surveys;
using CSA.Service.Application.Commands.Processes;
using CSA.Service.Application.DTOs;
using MediatR;

namespace CSA.Service.API.GraphQL;

public class CsaMutation
{
    public async Task<ControlDto> CreateControl(
        [Service] IMediator mediator,
        CreateControlDto input,
        CancellationToken ct) =>
        await mediator.Send(new CreateControlCommand(input, 0), ct);

    public async Task<ControlDto> UpdateControl(
        [Service] IMediator mediator,
        UpdateControlDto input,
        CancellationToken ct) =>
        await mediator.Send(new UpdateControlCommand(input, 0), ct);

    public async Task<bool> DeleteControl(
        [Service] IMediator mediator,
        long controlId,
        CancellationToken ct) =>
        await mediator.Send(new DeleteControlCommand(controlId), ct);

    public async Task<SurveyDto> CreateSurvey(
        [Service] IMediator mediator,
        CreateSurveyDto input,
        CancellationToken ct) =>
        await mediator.Send(new CreateSurveyCommand(input, 0), ct);

    public async Task<ProcessDto> CreateProcess(
        [Service] IMediator mediator,
        CreateProcessDto input,
        CancellationToken ct) =>
        await mediator.Send(new CreateProcessCommand(input, 0), ct);

    public async Task<SubProcessDto> CreateSubProcess(
        [Service] IMediator mediator,
        CreateSubProcessDto input,
        CancellationToken ct) =>
        await mediator.Send(new CreateSubProcessCommand(input, 0), ct);
}
