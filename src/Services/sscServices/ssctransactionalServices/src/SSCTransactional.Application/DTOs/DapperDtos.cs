namespace SSCTransactional.Application.DTOs;

public class AllocationFlatDto
{
    public long AllocationId { get; set; }
    public long DocId { get; set; }
    public string Action { get; set; } = "";
    public long GroupId { get; set; }
    public string PullStatus { get; set; } = "";
    public long PullUserId { get; set; }
    public int Priority { get; set; }
    public long AllocatedBy { get; set; }
    public DateTime AllocatedOn { get; set; }
    public string? Remarks { get; set; }
    public string ActionFlag { get; set; } = "";
    public DateTime? ActionDate { get; set; }
    public long? CorrespondenceId { get; set; }
    public long? DefectType { get; set; }
    public string? CloseRemarks { get; set; }
    public long ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public DateTime PulledOn { get; set; }
}

public class CorrespondenceFlatDto
{
    public long CorrespondenceId { get; set; }
    public long DocId { get; set; }
    public long AllocationId { get; set; }
    public long HoldCategory { get; set; }
    public long HoldType { get; set; }
    public DateTime HoldDate { get; set; }
    public string HoldRemarks { get; set; } = "";
    public long HoldBy { get; set; }
    public string HoldStatus { get; set; } = "";
    public DateTime? ReleaseDate { get; set; }
    public string? ReleaseRemarks { get; set; }
    public long? ReleasedBy { get; set; }
    public decimal? HoldNature { get; set; }
}

public class CorrespondenceAttachmentFlatDto
{
    public long AttachmentId { get; set; }
    public long CorrespondenceId { get; set; }
    public string Status { get; set; } = "";
    public string FilePath { get; set; } = "";
}
