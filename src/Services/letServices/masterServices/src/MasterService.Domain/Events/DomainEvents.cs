using MediatR;

namespace MasterService.Domain.Events;

public sealed record SkillCreatedEvent(long SkillCode, string SkillName, char SkillType) : INotification;
public sealed record SkillClosedEvent(long SkillCode) : INotification;
public sealed record TrainingProviderCreatedEvent(long TrainingCode, string TrainingName) : INotification;
public sealed record JobCreatedEvent(long JobCode, string JobName, string CategoryCode) : INotification;
public sealed record FinancialYearClosedEvent(long SerialNumber) : INotification;
