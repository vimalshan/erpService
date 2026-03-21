using InsuranceService.Domain.Common;

namespace InsuranceService.Domain.Events;

public record InsuranceRegisteredEvent(
    string CompanyCode,
    long PlanNumber,
    string InsuranceType) : DomainEvent;

public record InsuranceStatusChangedEvent(
    string CompanyCode,
    long PlanNumber,
    string OldStatus,
    string NewStatus) : DomainEvent;
