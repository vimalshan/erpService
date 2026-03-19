using FilingAndArchiveService.Domain.Common;
using FilingAndArchiveService.Domain.Events;
using FilingAndArchiveService.Domain.ValueObjects;

namespace FilingAndArchiveService.Domain.Entities;

public class FileMaster : BaseEntity
{
    // Private constructor for EF Core
    private FileMaster() { }

    public long FileId { get; private set; }
    public string FileOrgId { get; private set; } = default!;
    public long FileYear { get; private set; }
    public string FileNo { get; private set; } = default!;
    public string FileStatus { get; private set; } = default!;
    public string? FileRemarks { get; private set; }
    public string? FilePodNo { get; private set; }
    public string? FileCourierName { get; private set; }
    public DateTime FileCreatedOn { get; private set; }
    public long FileCreatedBy { get; private set; }
    public DateTime FileUpdatedOn { get; private set; }
    public long FileUpdatedBy { get; private set; }
    public DateTime? FileDispatchedOn { get; private set; }
    public long? FileDispatchedBy { get; private set; }

    public static FileMaster Create(
        long fileId,
        string fileOrgId,
        long fileYear,
        string fileNo,
        long createdBy,
        string? remarks = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileOrgId);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileNo);

        var file = new FileMaster
        {
            FileId = fileId,
            FileOrgId = fileOrgId,
            FileYear = fileYear,
            FileNo = fileNo,
            FileStatus = ValueObjects.FileStatus.Active.Code,
            FileRemarks = remarks,
            FileCreatedOn = DateTime.UtcNow,
            FileCreatedBy = createdBy,
            FileUpdatedOn = DateTime.UtcNow,
            FileUpdatedBy = createdBy
        };

        file.AddDomainEvent(new FileCreatedEvent(file.FileId, file.FileOrgId, file.FileNo));
        return file;
    }

    public void UpdateDetails(string? remarks, string? podNo, string? courierName, long updatedBy)
    {
        FileRemarks = remarks;
        FilePodNo = podNo;
        FileCourierName = courierName;
        FileUpdatedOn = DateTime.UtcNow;
        FileUpdatedBy = updatedBy;

        AddDomainEvent(new FileUpdatedEvent(FileId, FileStatus));
    }

    public void ChangeStatus(string newStatus, long updatedBy)
    {
        var status = ValueObjects.FileStatus.From(newStatus);
        var old = FileStatus;
        FileStatus = status.Code;
        FileUpdatedOn = DateTime.UtcNow;
        FileUpdatedBy = updatedBy;

        AddDomainEvent(new FileStatusChangedEvent(FileId, old, FileStatus));
    }

    public void Dispatch(string podNo, string courierName, long dispatchedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(podNo);
        ArgumentException.ThrowIfNullOrWhiteSpace(courierName);

        FilePodNo = podNo;
        FileCourierName = courierName;
        FileDispatchedOn = DateTime.UtcNow;
        FileDispatchedBy = dispatchedBy;
        FileStatus = ValueObjects.FileStatus.Dispatched.Code;
        FileUpdatedOn = DateTime.UtcNow;
        FileUpdatedBy = dispatchedBy;

        AddDomainEvent(new FileDispatchedEvent(FileId, FileOrgId, FileNo, podNo, courierName));
    }

    public void Close(long updatedBy) => ChangeStatus(ValueObjects.FileStatus.Closed.Code, updatedBy);

    public void Archive(long updatedBy) => ChangeStatus(ValueObjects.FileStatus.Archived.Code, updatedBy);
}
