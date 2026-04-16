namespace FindingsAPI.Gateway.Application.DTOs;

public class FindingDomainDto
{
    public int FindingId { get; set; }
    public string FindingNumber { get; set; } = string.Empty;
    public int AuditId { get; set; }
    public int? SiteId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FindingType { get; set; } = string.Empty;
    public string? Severity { get; set; }
    public int FindingStatusId { get; set; }
    public string? StatusName { get; set; }
    public int? FindingCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime IdentifiedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public bool IsActive { get; set; }
    public int? IdentifiedBy { get; set; }
    public int? AssignedTo { get; set; }
    public string? Evidence { get; set; }
    public string? RootCause { get; set; }
    public string? CorrectiveAction { get; set; }
    public string? PreventiveAction { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? VerificationDate { get; set; }
    public int? VerifiedBy { get; set; }
}

public class CreateFindingDomainDto
{
    public int AuditId { get; set; }
    public int? SiteId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string FindingType { get; set; } = string.Empty;
    public string? Severity { get; set; }
    public int FindingStatusId { get; set; }
    public int? FindingCategoryId { get; set; }
    public int? IdentifiedBy { get; set; }
}

public class UpdateFindingDomainDto
{
    public int FindingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Severity { get; set; }
    public int? FindingCategoryId { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Evidence { get; set; }
    public string? RootCause { get; set; }
    public string? CorrectiveAction { get; set; }
    public string? PreventiveAction { get; set; }
    public int? ModifiedBy { get; set; }
}

public class FindingResponseDto
{
    public int FindingResponseId { get; set; }
    public int FindingId { get; set; }
    public string ResponseText { get; set; } = string.Empty;
    public string ResponseType { get; set; } = string.Empty;
    public DateTime ResponseDate { get; set; }
    public int RespondedBy { get; set; }
    public bool IsSubmittedToDNV { get; set; }
    public string? Status { get; set; }
    public string? AttachmentPath { get; set; }
}

public class CreateFindingResponseDto
{
    public int FindingId { get; set; }
    public string ResponseText { get; set; } = string.Empty;
    public string ResponseType { get; set; } = string.Empty;
    public int RespondedBy { get; set; }
    public string? AttachmentPath { get; set; }
}

public class FindingStatusDto
{
    public int FindingStatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string StatusCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Color { get; set; }
    public int? DisplayOrder { get; set; }
    public bool IsClosedStatus { get; set; }
}

public class FindingCategoryDto
{
    public int FindingCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? Color { get; set; }
    public int? DisplayOrder { get; set; }
}
