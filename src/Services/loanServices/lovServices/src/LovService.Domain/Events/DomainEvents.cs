using LovService.Domain.Common;
using LovService.Domain.Entities;

namespace LovService.Domain.Events;

public sealed record LovTypeCreatedEvent(LovTypeMast LovType) : IDomainEvent;
public sealed record LovTypeUpdatedEvent(LovTypeMast LovType) : IDomainEvent;
public sealed record LovMasterCreatedEvent(LovMaster LovMaster) : IDomainEvent;
public sealed record LovMasterUpdatedEvent(LovMaster LovMaster) : IDomainEvent;
public sealed record LovMasterDeletedEvent(long LovId, int LovTypeId) : IDomainEvent;
public sealed record ProgramLovCreatedEvent(ProgramLovMast ProgramLov) : IDomainEvent;
public sealed record ProgramLovUpdatedEvent(ProgramLovMast ProgramLov) : IDomainEvent;
