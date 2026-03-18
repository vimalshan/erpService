using EximManagement.Domain.Common;

namespace EximManagement.Domain.Events;

public record EximDataFileUploadedEvent(long FileId, string FileType, DateTime OccurredOn) : IDomainEvent;

public record EximDataFileDeletedEvent(long FileId, DateTime OccurredOn) : IDomainEvent;

public record EximProductCreatedEvent(long ProductId, string ProductName, DateTime OccurredOn) : IDomainEvent;

public record EximProductGroupCreatedEvent(long GroupId, string GroupName, DateTime OccurredOn) : IDomainEvent;

public record EximDataProcessedEvent(long FileId, string FileType, int RecordCount, DateTime OccurredOn) : IDomainEvent;
