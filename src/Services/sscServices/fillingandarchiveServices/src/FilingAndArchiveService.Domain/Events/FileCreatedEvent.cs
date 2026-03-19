using FilingAndArchiveService.Domain.Common;

namespace FilingAndArchiveService.Domain.Events;

public sealed class FileCreatedEvent : DomainEvent
{
    public FileCreatedEvent(long fileId, string orgId, string fileNo)
    {
        FileId = fileId;
        OrgId = orgId;
        FileNo = fileNo;
    }

    public long FileId { get; }
    public string OrgId { get; }
    public string FileNo { get; }
}
