using ContractService.Application.Commands;
using ContractService.Application.DTOs;
using MediatR;

namespace ContractService.GraphQL.Mutations;

public class Mutation
{
    public async Task<ContractDto> CreateContract([Service] IMediator mediator, CreateContractDto input)
        => await mediator.Send(new CreateContractCommand(input));

    public async Task<ContractDto> UpdateContract([Service] IMediator mediator, UpdateContractDto input)
        => await mediator.Send(new UpdateContractCommand(input));

    public async Task<bool> DeleteContract([Service] IMediator mediator, int contractId)
        => await mediator.Send(new DeleteContractCommand(contractId));

    public async Task<ContractDto> ChangeContractStatus([Service] IMediator mediator, int contractId, string newStatus, int? modifiedBy)
        => await mediator.Send(new ChangeContractStatusCommand(contractId, newStatus, modifiedBy));

    public async Task<ContractDto> RenewContract([Service] IMediator mediator, int contractId, DateTime? newEndDate, int? modifiedBy)
        => await mediator.Send(new RenewContractCommand(contractId, newEndDate, modifiedBy));
}
