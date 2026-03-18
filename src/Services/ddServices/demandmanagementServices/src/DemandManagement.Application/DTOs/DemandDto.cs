namespace DemandManagement.Application.DTOs;

public class DemandDto
{
    public long DemandId { get; set; }
    public string DemandType { get; set; }
    public long DepartmentId { get; set; }
    public string DemandDescription { get; set; }
    public DateTime? RequiredDate { get; set; }
    public string Priority { get; set; }
    public string DemandStatus { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ApprovalRemarks { get; set; }
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string CompletionRemarks { get; set; }
    public long? CompletedBy { get; set; }
    public DateTime? CompletionDate { get; set; }
}

public class CreateDemandRequest
{
    public string DemandType { get; set; }
    public long DepartmentId { get; set; }
    public string DemandDescription { get; set; }
    public DateTime? RequiredDate { get; set; }
    public string Priority { get; set; }
    public long CreatedBy { get; set; }
}
