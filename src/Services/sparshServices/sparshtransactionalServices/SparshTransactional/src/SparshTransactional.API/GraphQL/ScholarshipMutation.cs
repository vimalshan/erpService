using MediatR;
using SparshTransactional.Application.Commands;
using SparshTransactional.Application.DTOs;

namespace SparshTransactional.API.GraphQL;

public class ScholarshipMutation
{
    public async Task<ScholarshipMasterDto> CreateScholarship(
        [Service] IMediator mediator,
        CreateScholarshipCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<ScholarshipMasterDto> UpdateScholarship(
        [Service] IMediator mediator,
        UpdateScholarshipCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<EligibilityCriteriaDto> AddEligibilityCriteria(
        [Service] IMediator mediator,
        AddEligibilityCriteriaCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<ScholarshipApplicationDto> SubmitApplication(
        [Service] IMediator mediator,
        SubmitApplicationCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<ScholarshipApplicationDto> ApproveApplication(
        [Service] IMediator mediator,
        ApproveApplicationCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<ScholarshipApplicationDto> RejectApplication(
        [Service] IMediator mediator,
        RejectApplicationCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<ScholarshipDisbursementDto> CreateDisbursement(
        [Service] IMediator mediator,
        CreateDisbursementCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);

    public async Task<ScholarshipDisbursementDto> CompleteDisbursement(
        [Service] IMediator mediator,
        CompleteDisbursementCommand input,
        CancellationToken ct) =>
        await mediator.Send(input, ct);
}
