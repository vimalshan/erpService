using DeductionService.Application.CQRS.Commands.CancelDeduction;
using DeductionService.Application.CQRS.Commands.CreateAdhocDeduction;
using DeductionService.Application.CQRS.Commands.ProcessMonthlyDeduction;
using DeductionService.Application.DTOs;
using MediatR;

namespace DeductionService.API.GraphQL.Mutations;

[MutationType]
public class DeductionMutation
{
    public async Task<AdhocPayDeductionDto> CreateDeductionAsync(
        CreateAdhocDeductionDto input,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new CreateAdhocDeductionCommand(
            input.SystemId, input.CanteenUnit, input.PayAmount,
            input.EarningDeductionCode, input.EmployeeNumber,
            input.EnteredByUserId, input.CompanyCode, input.GradeType), ct);

    public async Task<bool> CancelDeductionAsync(
        long systemId,
        long cancelledByUserId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new CancelDeductionCommand(systemId, cancelledByUserId), ct);

    public async Task<ProcessMonthlyDeductionResultDto> ProcessMonthlyDeductionAsync(
        string monthYear,
        long processedByUserId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new ProcessMonthlyDeductionCommand(monthYear, processedByUserId), ct);
}
