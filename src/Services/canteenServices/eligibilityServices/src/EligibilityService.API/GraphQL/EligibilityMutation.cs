using EligibilityService.Application.Commands.EligibilityMaster;
using EligibilityService.Application.DTOs;
using MediatR;

namespace EligibilityService.API.GraphQL;

public class EligibilityMutation
{
    [GraphQLDescription("Create a new eligibility master record.")]
    public async Task<EligibilityMasterDto> CreateEligibility(
        [Service] IMediator mediator,
        CreateEligibilityMasterCommand input,
        CancellationToken ct)
        => await mediator.Send(input, ct);

    [GraphQLDescription("Update an existing eligibility master record.")]
    public async Task<EligibilityMasterDto> UpdateEligibility(
        [Service] IMediator mediator,
        UpdateEligibilityMasterCommand input,
        CancellationToken ct)
        => await mediator.Send(input, ct);

    [GraphQLDescription("Delete an eligibility master record.")]
    public async Task<bool> DeleteEligibility(
        [Service] IMediator mediator,
        long canteenUnit,
        string shiftCode,
        decimal itemCode,
        CancellationToken ct)
        => await mediator.Send(new DeleteEligibilityMasterCommand(canteenUnit, shiftCode, itemCode), ct);
}
