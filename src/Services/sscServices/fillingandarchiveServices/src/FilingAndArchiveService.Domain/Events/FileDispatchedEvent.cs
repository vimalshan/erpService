using FilingAndArchiveService.Domain.Common;

namespace FilingAndArchiveService.Domain.Events;

public sealed class FileDispatchedEvent : DomainEvent
{
    public FileDispatchedEvent(long fileId, string orgId, string fileNo, string podNo, string courierName)
    {
        FileId = fileId;
        OrgId = orgId;
        FileNo = fileNo;
        PodNo = podNo;
        CourierName = courierName;
    }

    public long FileId { get; }
    public string OrgId { get; }
    public string FileNo { get; }
    public string PodNo { get; }
    public string CourierName { get; }
}
