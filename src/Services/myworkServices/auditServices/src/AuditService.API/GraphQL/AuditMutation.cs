using AuditService.Application.Commands.Audits;
using AuditService.Application.Commands.Observations;
using AuditService.Application.Commands.GoodPractices;
using AuditService.Application.DTOs;
using MediatR;

namespace AuditService.API.GraphQL;

public class AuditMutation
{
    public async Task<AuditDto> CreateAudit([Service] ISender sender, CreateAuditCommand input, CancellationToken cancellationToken)
        => await sender.Send(input, cancellationToken);

    public async Task<bool> UpdateAudit([Service] ISender sender, UpdateAuditCommand input, CancellationToken cancellationToken)
        => await sender.Send(input, cancellationToken);

    public async Task<ObservationDto> CreateObservation([Service] ISender sender, CreateObservationCommand input, CancellationToken cancellationToken)
        => await sender.Send(input, cancellationToken);

    public async Task<bool> UpdateObservationStatus([Service] ISender sender, UpdateObservationStatusCommand input, CancellationToken cancellationToken)
        => await sender.Send(input, cancellationToken);

    public async Task<GoodPracticeDto> CreateGoodPractice([Service] ISender sender, CreateGoodPracticeCommand input, CancellationToken cancellationToken)
        => await sender.Send(input, cancellationToken);

    public async Task<bool> RateGoodPractice([Service] ISender sender, RateGoodPracticeCommand input, CancellationToken cancellationToken)
        => await sender.Send(input, cancellationToken);
}
