using MediatR;
using ScholarshipService.Application.Commands.ApproveScholarship;
using ScholarshipService.Application.Commands.CreateScholarship;
using ScholarshipService.Application.Commands.StopScholarship;

namespace ScholarshipService.API.GraphQL.Mutations;

public class ScholarshipMutation
{
    [GraphQLDescription("Submit a new scholarship application.")]
    public async Task<int> CreateScholarship(
        [Service] IMediator mediator,
        CreateScholarshipCommand input)
        => await mediator.Send(input);

    [GraphQLDescription("Approve a pending scholarship application.")]
    public async Task<bool> ApproveScholarship(
        [Service] IMediator mediator,
        int scholarshipId,
        int approvedBy,
        string? remarks = null)
        => await mediator.Send(new ApproveScholarshipCommand(scholarshipId, approvedBy, remarks));

    [GraphQLDescription("Stop an active scholarship.")]
    public async Task<bool> StopScholarship(
        [Service] IMediator mediator,
        int scholarshipId,
        string reason,
        int stoppedBy)
        => await mediator.Send(new StopScholarshipCommand(scholarshipId, reason, stoppedBy));
}
