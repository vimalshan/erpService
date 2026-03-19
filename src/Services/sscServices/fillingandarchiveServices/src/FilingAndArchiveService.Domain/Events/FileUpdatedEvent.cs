using FilingAndArchiveService.Domain.Common;

namespace FilingAndArchiveService.Domain.Events;

public sealed class FileUpdatedEvent : DomainEvent
{
    public FileUpdatedEvent(long fileId, string status)
    {
        FileId = fileId;
        Status = status;
    }

    public long FileId { get; }
    public string Status { get; }
}
