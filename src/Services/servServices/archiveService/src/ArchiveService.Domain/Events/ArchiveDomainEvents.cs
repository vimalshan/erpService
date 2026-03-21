using ArchiveService.Domain.Common;

namespace ArchiveService.Domain.Events;

public record ServiceOrderArchivedEvent(string SernoDell, string? SapId) : DomainEvent;

public record ServiceOrderStatusChangedEvent(string SernoDell, string? NewStatus) : DomainEvent;

public record ToolKitArchivedEvent(string? KitCode, string? EngineerId) : DomainEvent;

public record ArchiveDataPurgedEvent(string TableName, int RecordsPurged) : DomainEvent;
