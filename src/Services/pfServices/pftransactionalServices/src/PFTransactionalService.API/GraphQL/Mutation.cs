using MediatR;
using PFTransactionalService.Application.Commands.ApplyInterest;
using PFTransactionalService.Application.Commands.GenerateCertificate;
using PFTransactionalService.Application.Commands.ProcessContribution;
using PFTransactionalService.Application.Commands.ProcessWithdrawal;
using PFTransactionalService.Application.DTOs;

namespace PFTransactionalService.API.GraphQL;

public class Mutation
{
    public async Task<PFAccumulationDto> ProcessContribution(
        [Service] IMediator mediator,
        ProcessContributionCommand input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<PFAccumulationDto> ProcessWithdrawal(
        [Service] IMediator mediator,
        ProcessWithdrawalCommand input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<PFAccumulationDto> ApplyInterest(
        [Service] IMediator mediator,
        ApplyInterestCommand input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }

    public async Task<WithdrawalCertificateDto> GenerateCertificate(
        [Service] IMediator mediator,
        GenerateCertificateCommand input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(input, cancellationToken);
    }
}
