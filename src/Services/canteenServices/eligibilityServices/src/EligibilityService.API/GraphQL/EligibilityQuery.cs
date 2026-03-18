using EligibilityService.Domain.Entities;
using EligibilityService.Application.DTOs;
using EligibilityService.Domain.Interfaces;
using MediatR;
using EligibilityService.Application.Queries.EligibilityMaster;

namespace EligibilityService.API.GraphQL;

public class EligibilityQuery
{
    [GraphQLDescription("Get all eligibility master records, optionally filtered by canteen unit.")]
    public async Task<IEnumerable<EligibilityMasterDto>> GetEligibilityMasters(
        [Service] IMediator mediator,
        long? canteenUnit,
        CancellationToken ct)
        => await mediator.Send(new GetAllEligibilityMastersQuery(canteenUnit), ct);

    [GraphQLDescription("Get a single eligibility master by composite key.")]
    public async Task<EligibilityMasterDto?> GetEligibilityMaster(
        [Service] IMediator mediator,
        long canteenUnit,
        string shiftCode,
        decimal itemCode,
        CancellationToken ct)
        => await mediator.Send(new GetEligibilityMasterQuery(canteenUnit, shiftCode, itemCode), ct);

    [GraphQLDescription("Check meal eligibility for a given shift/item combination.")]
    public async Task<EligibilityCheckResultDto> CheckEligibility(
        [Service] IMediator mediator,
        long canteenUnit,
        string shiftCode,
        decimal itemCode,
        int requestedQty,
        CancellationToken ct)
        => await mediator.Send(new CheckEmployeeEligibilityQuery(canteenUnit, shiftCode, itemCode, requestedQty), ct);

    [GraphQLDescription("Get eligibility audit history.")]
    public async Task<IEnumerable<EligibilityMasterHistoryDto>> GetEligibilityHistory(
        [Service] IMediator mediator,
        long canteenUnit,
        string shiftCode,
        decimal itemCode,
        CancellationToken ct)
        => await mediator.Send(new GetEligibilityHistoryQuery(canteenUnit, shiftCode, itemCode), ct);
}
