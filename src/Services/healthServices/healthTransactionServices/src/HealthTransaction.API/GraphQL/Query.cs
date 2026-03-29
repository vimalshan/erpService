using HotChocolate.Authorization;
using HealthTransaction.Application.DTOs;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetAll;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetByEmployeeNum;
using HealthTransaction.Application.Features.PreEmploymentCheckups.Queries.GetByDateRange;
using HealthTransaction.Application.Features.CheckupCards.Queries.GetAll;
using HealthTransaction.Application.Features.CheckupCards.Queries.GetByHlthNum;
using HealthTransaction.Application.Features.DynamicHealthDetails.Queries.GetByHlthNum;
using HealthTransaction.Application.Features.PfiHistories.Queries.GetByHlthNum;
using MediatR;

namespace HealthTransaction.API.GraphQL;

[Authorize]
public class Query
{
    public async Task<IReadOnlyList<PreEmploymentCheckupDto>> GetPreEmploymentCheckups(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllPreEmploymentCheckupsQuery(), ct);

    public async Task<IReadOnlyList<PreEmploymentCheckupDto>> GetPreEmploymentCheckupsByEmployee(
        decimal empNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetPreEmploymentCheckupsByEmployeeNumQuery(empNum), ct);

    public async Task<IReadOnlyList<PreEmploymentCheckupDto>> GetPreEmploymentCheckupsByDateRange(
        DateTime from, DateTime to, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetPreEmploymentCheckupsByDateRangeQuery(from, to), ct);

    public async Task<IReadOnlyList<CheckupCardDto>> GetCheckupCards(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllCheckupCardsQuery(), ct);

    public async Task<CheckupCardDto?> GetCheckupCard(
        decimal hlthNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetCheckupCardByHlthNumQuery(hlthNum), ct);

    public async Task<IReadOnlyList<DynamicHealthDetailDto>> GetDynamicHealthDetails(
        decimal hlthNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetDynamicHealthDetailsByHlthNumQuery(hlthNum), ct);

    public async Task<IReadOnlyList<PfiHistoryDto>> GetPfiHistories(
        decimal hlthNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetPfiHistoriesByHlthNumQuery(hlthNum), ct);
}
