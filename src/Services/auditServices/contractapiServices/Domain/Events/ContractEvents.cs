using MediatR;

namespace ContractService.Domain.Events;

public interface IDomainEvent : INotification { }

public record ContractCreatedEvent(int ContractId, string ContractNumber, string ContractName, int CompanyId) : IDomainEvent;
public record ContractStatusChangedEvent(int ContractId, string OldStatus, string NewStatus) : IDomainEvent;
public record ContractRenewedEvent(int ContractId, string ContractNumber, DateTime? NewEndDate) : IDomainEvent;
public record ContractCancelledEvent(int ContractId, string ContractNumber, string Reason) : IDomainEvent;
