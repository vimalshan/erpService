using FilingAndArchiveService.Domain.Common;

namespace FilingAndArchiveService.Domain.Events;

public sealed class FileStatusChangedEvent : DomainEvent
{
    public FileStatusChangedEvent(long fileId, string oldStatus, string newStatus)
    {
        FileId = fileId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }

    public long FileId { get; }
    public string OldStatus { get; }
    public string NewStatus { get; }
}
