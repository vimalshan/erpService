using FindingsAPI.Gateway.Domain.Events;

namespace FindingsAPI.Gateway.Domain.Entities;

public class FindingEntity
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
    public int? FindingCategoryId { get; set; }
    public DateTime IdentifiedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public int? IdentifiedBy { get; set; }
    public int? AssignedTo { get; set; }
    public string? Evidence { get; set; }
    public string? RootCause { get; set; }
    public string? CorrectiveAction { get; set; }
    public string? PreventiveAction { get; set; }
    public string? VerificationMethod { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? VerificationDate { get; set; }
    public int? VerifiedBy { get; set; }

    // Navigation
    public FindingStatusEntity? FindingStatus { get; set; }
    public FindingCategoryEntity? FindingCategory { get; set; }
    public ICollection<FindingResponseEntity> Responses { get; set; } = new List<FindingResponseEntity>();
    public ICollection<FindingClauseEntity> Clauses { get; set; } = new List<FindingClauseEntity>();
    public ICollection<FindingFocusAreaEntity> FocusAreas { get; set; } = new List<FindingFocusAreaEntity>();

    private readonly List<MediatR.INotification> _domainEvents = new();
    public IReadOnlyCollection<MediatR.INotification> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();

    public static FindingEntity Create(int auditId, string title, string description, string findingType,
        string? severity, int findingStatusId, int? findingCategoryId, int? siteId, int? identifiedBy)
    {
        var entity = new FindingEntity
        {
            AuditId = auditId,
            Title = title,
            Description = description,
            FindingType = findingType,
            Severity = severity,
            FindingStatusId = findingStatusId,
            FindingCategoryId = findingCategoryId,
            SiteId = siteId,
            IdentifiedBy = identifiedBy,
            IdentifiedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = identifiedBy
        };
        entity._domainEvents.Add(new FindingCreatedEvent(0, entity.FindingNumber, auditId, title, findingType));
        return entity;
    }

    public void ChangeStatus(int newStatusId, int? modifiedBy)
    {
        var oldStatusId = FindingStatusId;
        FindingStatusId = newStatusId;
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
        _domainEvents.Add(new FindingStatusChangedEvent(FindingId, oldStatusId, newStatusId));
    }

    public void Close(int? closedBy)
    {
        ClosedDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = closedBy;
        _domainEvents.Add(new FindingClosedEvent(FindingId, ClosedDate.Value, closedBy));
    }

    public void Assign(int? assignedTo, int? modifiedBy)
    {
        AssignedTo = assignedTo;
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
        _domainEvents.Add(new FindingAssignedEvent(FindingId, assignedTo));
    }

    public void Verify(int? verifiedBy)
    {
        VerifiedBy = verifiedBy;
        VerificationDate = DateTime.UtcNow;
        ModifiedDate = DateTime.UtcNow;
        _domainEvents.Add(new FindingVerifiedEvent(FindingId, verifiedBy, VerificationDate.Value));
    }
}

public class FindingStatusEntity
{
    public int FindingStatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string StatusCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Color { get; set; }
    public int? DisplayOrder { get; set; }
    public bool IsClosedStatus { get; set; }
}

public class FindingCategoryEntity
{
    public int FindingCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? Color { get; set; }
    public int? DisplayOrder { get; set; }

    public FindingCategoryEntity? ParentCategory { get; set; }
    public ICollection<FindingCategoryEntity> ChildCategories { get; set; } = new List<FindingCategoryEntity>();
}

public class FindingClauseEntity
{
    public int FindingClauseId { get; set; }
    public int FindingId { get; set; }
    public int ClauseId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Notes { get; set; }

    public FindingEntity? Finding { get; set; }
}

public class FindingFocusAreaEntity
{
    public int FindingFocusAreaId { get; set; }
    public int FindingId { get; set; }
    public int FocusAreaId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Notes { get; set; }

    public FindingEntity? Finding { get; set; }
}

public class FindingResponseEntity
{
    public int FindingResponseId { get; set; }
    public int FindingId { get; set; }
    public string ResponseText { get; set; } = string.Empty;
    public string ResponseType { get; set; } = string.Empty;
    public DateTime ResponseDate { get; set; }
    public int RespondedBy { get; set; }
    public bool IsSubmittedToDNV { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? AttachmentPath { get; set; }
    public string? Status { get; set; }
    public string? ReviewComments { get; set; }
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewDate { get; set; }

    public FindingEntity? Finding { get; set; }
}
