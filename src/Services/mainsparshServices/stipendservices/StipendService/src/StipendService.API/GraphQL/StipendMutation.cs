using MediatR;
using StipendService.Application.DTOs;
using StipendService.Application.Features.StipendMaster.Commands;
using StipendService.Application.Features.StipendDisbursement.Commands;

namespace StipendService.API.GraphQL;

public class StipendMutation
{
    public async Task<StipendMasterDto> CreateStipendMaster(
        CreateStipendMasterCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(input, cancellationToken);

    public async Task<ProcessMonthlyStipendResultDto> ProcessMonthlyStipend(
        ProcessMonthlyStipendCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(input, cancellationToken);

    public async Task<CalculateDisbursementResultDto> CalculateAndDisburse(
        CalculateAndDisburseStipendCommand input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(input, cancellationToken);
}
