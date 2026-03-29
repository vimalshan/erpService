using HotChocolate.Authorization;
using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Commands.Create;
using HealthTransaction.Application.Features.CheckupCards.Commands.Create;
using HealthTransaction.Application.Features.DynamicHealthDetails.Commands.Save;
using HealthTransaction.Application.Features.PfiHistories.Commands.Create;
using MediatR;

namespace HealthTransaction.API.GraphQL;

[Authorize]
public class Mutation
{
    public async Task<PreEmploymentCheckupDto> CreatePreEmploymentCheckup(
        CreatePreEmploymentCheckupDto input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreatePreEmploymentCheckupCommand(input), ct);

    public async Task<CheckupCardDto> CreateCheckupCard(
        CreateCheckupCardDto input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateCheckupCardCommand(input), ct);

    public async Task<IList<DynamicHealthDetailDto>> SaveDynamicHealthDetails(
        IList<SaveDynamicHealthDetailDto> items, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new SaveDynamicHealthDetailsCommand(items), ct);

    public async Task<PfiHistoryDto> CreatePfiHistory(
        CreatePfiHistoryDto input, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreatePfiHistoryCommand(input), ct);
}
