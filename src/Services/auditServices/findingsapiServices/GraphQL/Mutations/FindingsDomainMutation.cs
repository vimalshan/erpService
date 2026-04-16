using FindingsAPI.Gateway.Application.Commands;
using FindingsAPI.Gateway.Application.DTOs;
using FindingsAPI.Gateway.Application.Queries;
using HotChocolate.Authorization;
using MediatR;

namespace FindingsAPI.Gateway.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class FindingsDomainMutation
{
    [GraphQLDescription("Create a finding via domain layer")]
    [Authorize(Policy = "Auditor")]
    public async Task<FindingDomainDto> CreateDomainFinding(
        CreateFindingDomainDto input,
        [Service] IMediator mediator) =>
        await mediator.Send(new CreateFindingDomainCommand(input));

    [GraphQLDescription("Update a finding via domain layer")]
    [Authorize(Policy = "Auditor")]
    public async Task<FindingDomainDto> UpdateDomainFinding(
        UpdateFindingDomainDto input,
        [Service] IMediator mediator) =>
        await mediator.Send(new UpdateFindingDomainCommand(input));

    [GraphQLDescription("Delete a finding (soft)")]
    [Authorize(Policy = "Admin")]
    public async Task<bool> DeleteDomainFinding(
        int findingId,
        [Service] IMediator mediator) =>
        await mediator.Send(new DeleteFindingDomainCommand(findingId));

    [GraphQLDescription("Change finding status")]
    [Authorize(Policy = "Auditor")]
    public async Task<FindingDomainDto> ChangeFindingStatus(
        int findingId, int newStatusId, int? modifiedBy,
        [Service] IMediator mediator) =>
        await mediator.Send(new ChangeStatusCommand(findingId, newStatusId, modifiedBy));

    [GraphQLDescription("Close a finding via domain layer")]
    [Authorize(Policy = "Admin")]
    public async Task<FindingDomainDto> CloseDomainFinding(
        int findingId, int? closedBy,
        [Service] IMediator mediator) =>
        await mediator.Send(new CloseFindingDomainCommand(findingId, closedBy));

    [GraphQLDescription("Assign a finding to a user")]
    [Authorize(Policy = "Auditor")]
    public async Task<FindingDomainDto> AssignFinding(
        int findingId, int? assignedTo, int? modifiedBy,
        [Service] IMediator mediator) =>
        await mediator.Send(new AssignFindingCommand(findingId, assignedTo, modifiedBy));

    [GraphQLDescription("Verify a finding")]
    [Authorize(Policy = "Admin")]
    public async Task<FindingDomainDto> VerifyFinding(
        int findingId, int? verifiedBy,
        [Service] IMediator mediator) =>
        await mediator.Send(new VerifyFindingCommand(findingId, verifiedBy));

    [GraphQLDescription("Add a response to a finding")]
    [Authorize(Policy = "Auditor")]
    public async Task<FindingResponseDto> AddFindingResponse(
        CreateFindingResponseDto input,
        [Service] IMediator mediator) =>
        await mediator.Send(new AddFindingResponseCommand(input));

    [GraphQLDescription("Get finding statuses")]
    public async Task<IEnumerable<FindingStatusDto>> GetFindingStatuses(
        [Service] IMediator mediator) =>
        await mediator.Send(new GetFindingStatusesQuery());

    [GraphQLDescription("Get finding categories")]
    public async Task<IEnumerable<FindingCategoryDto>> GetFindingCategories(
        [Service] IMediator mediator) =>
        await mediator.Send(new GetFindingCategoriesQuery());

    [GraphQLDescription("Get finding responses")]
    public async Task<IEnumerable<FindingResponseDto>> GetFindingResponses(
        int findingId,
        [Service] IMediator mediator) =>
        await mediator.Send(new GetFindingResponsesQuery(findingId));
}
