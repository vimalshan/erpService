namespace ApprovalService.Application.DTOs;

/// <summary>
/// DTO for Approval Master
/// </summary>
public record ApprovalMasterDto
{
    public long Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Module { get; set; }
    public int Level { get; set; }
    public string Status { get; set; } = "Active";
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public List<ApproverEmployeeDto> Approvers { get; set; } = [];
}

/// <summary>
/// DTO for Approver Employee
/// </summary>
public record ApproverEmployeeDto
{
    public long Id { get; set; }
    public long ApprovalMasterId { get; set; }
    public long EmployeeSysId { get; set; }
    public int ApproverLevel { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}

/// <summary>
/// DTO for creating approval master
/// </summary>
public record CreateApprovalMasterDto
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Module { get; set; }
    public int Level { get; set; } = 1;
}

/// <summary>
/// DTO for updating approval master
/// </summary>
public record UpdateApprovalMasterDto
{
    public required string Name { get; set; }
    public int Level { get; set; }
}

/// <summary>
/// DTO for creating approver employee
/// </summary>
public record CreateApproverEmployeeDto
{
    public long ApprovalMasterId { get; set; }
    public long EmployeeSysId { get; set; }
    public int ApproverLevel { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
}

/// <summary>
/// DTO for updating approver employee
/// </summary>
public record UpdateApproverEmployeeDto
{
    public int ApproverLevel { get; set; }
    public DateTime? EffectiveTo { get; set; }
}

/// <summary>
/// Response DTO for API responses
/// </summary>
public record ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public T? Data { get; set; }
    public Dictionary<string, string[]> Errors { get; set; } = [];
}

/// <summary>
/// Pagination DTO
/// </summary>
public record PaginatedDto<T>
{
    public List<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool HasMore => (PageNumber * PageSize) < TotalCount;
}
