namespace FilingAndArchiveService.Application.DTOs;

public class FileMasterDto
{
    public long FileId { get; set; }
    public string FileOrgId { get; set; } = default!;
    public long FileYear { get; set; }
    public string FileNo { get; set; } = default!;
    public string FileStatus { get; set; } = default!;
    public string? FileRemarks { get; set; }
    public string? FilePodNo { get; set; }
    public string? FileCourierName { get; set; }
    public DateTime FileCreatedOn { get; set; }
    public long FileCreatedBy { get; set; }
    public DateTime FileUpdatedOn { get; set; }
    public long FileUpdatedBy { get; set; }
    public DateTime? FileDispatchedOn { get; set; }
    public long? FileDispatchedBy { get; set; }
}
