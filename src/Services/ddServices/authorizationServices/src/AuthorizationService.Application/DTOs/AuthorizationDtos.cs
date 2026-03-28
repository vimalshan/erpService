namespace AuthorizationService.Application.DTOs;

public class RightDto
{
    public long Id { get; set; }
    public decimal RightCode { get; set; }
    public string? RightDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SpecialInputDto
{
    public long Id { get; set; }
    public decimal SpecialInputId { get; set; }
    public decimal YearId { get; set; }
    public string? RoleType { get; set; }
    public decimal EmployeeSysId { get; set; }
    public decimal AppraisalSysId { get; set; }
    public string? Inputs { get; set; }
    public char Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? SubmittedOn { get; set; }
    public bool IsSubmitted { get; set; }
}

public class SpecialInputMasterDto
{
    public long Id { get; set; }
    public decimal SpecialInputId { get; set; }
    public decimal YearId { get; set; }
    public string? RoleType { get; set; }
    public decimal EmployeeSysId { get; set; }
    public decimal AppraisalSysId { get; set; }
    public decimal CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class TrackerRightDto
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public decimal? PinNumber { get; set; }
    public string? TrackerMode { get; set; }
    public string? BusinessCode { get; set; }
    public string? UnitCode { get; set; }
    public char? TrackerRights { get; set; }
    public char? VtcRights { get; set; }
    public char? RepresentingUnit { get; set; }
    public char? LetRight { get; set; }
    public char? CarRight { get; set; }
    public bool HasTrackerAccess { get; set; }
    public bool HasVtcAccess { get; set; }
}

public class UserRightDto
{
    public long Id { get; set; }
    public string? UserId { get; set; }
    public decimal? PinNumber { get; set; }
    public decimal? RightCode { get; set; }
    public string? BusinessCode { get; set; }
    public string? UnitCode { get; set; }
    public decimal? RightMode { get; set; }
}
