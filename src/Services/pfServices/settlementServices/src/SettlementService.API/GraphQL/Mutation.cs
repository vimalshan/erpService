using MediatR;
using SettlementService.Application.Commands.ApproveSettlement;
using SettlementService.Application.Commands.CreateSettlement;
using SettlementService.Application.Commands.RejectSettlement;
using SettlementService.Application.DTOs;

namespace SettlementService.API.GraphQL;

public class Mutation
{
    public async Task<SettlementDto> CreateSettlement(
        [Service] IMediator mediator,
        CreateSettlementCommand input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<SettlementDto> ApproveSettlement(
        [Service] IMediator mediator,
        ApproveSettlementCommand input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<SettlementDto> RejectSettlement(
        [Service] IMediator mediator,
        RejectSettlementCommand input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }
}
