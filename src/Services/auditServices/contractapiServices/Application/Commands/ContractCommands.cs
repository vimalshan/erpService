using ContractService.Application.DTOs;
using MediatR;

namespace ContractService.Application.Commands;

public record CreateContractCommand(CreateContractDto Dto) : IRequest<ContractDto>;
public record UpdateContractCommand(UpdateContractDto Dto) : IRequest<ContractDto>;
public record DeleteContractCommand(int ContractId) : IRequest<bool>;
public record ChangeContractStatusCommand(int ContractId, string NewStatus, int? ModifiedBy) : IRequest<ContractDto>;
public record RenewContractCommand(int ContractId, DateTime? NewEndDate, int? ModifiedBy) : IRequest<ContractDto>;
