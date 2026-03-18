using MediatR;

namespace CompensationBenefits.Domain.Events;

public record SalaryCreatedDomainEvent(long SalaryId, decimal CtcAmount) : INotification;

public record SalaryCancelledDomainEvent(long SalaryId) : INotification;

public record SalaryStructureCreatedDomainEvent(long StructureId, string Name) : INotification;

public record MediclaimUpdatedDomainEvent(long MediclaimId, string? RefName) : INotification;

public record MobileConnectionCreatedDomainEvent(long ConnectionId, long EmpSysId) : INotification;
