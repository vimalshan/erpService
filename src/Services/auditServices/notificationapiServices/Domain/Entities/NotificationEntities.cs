using NotificationService.Domain.Events;

namespace NotificationService.Domain.Entities;

public class Notification
{
    public int NotificationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int? CompanyId { get; set; }
    public int? SiteId { get; set; }
    public int? ServiceId { get; set; }
    public string Priority { get; set; } = "Medium";
    public string Status { get; set; } = "Active";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ReadBy { get; set; }
    public string? TargetAudience { get; set; }
    public bool ActionRequired { get; set; }
    public string? ActionUrl { get; set; }
    public string? AttachmentPath { get; set; }
    public string? RelatedEntityType { get; set; }
    public int? RelatedEntityId { get; set; }

    public NotificationCategory Category { get; set; } = null!;

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();

    public static Notification Create(string title, string message, int categoryId, string priority,
        int? companyId, int? siteId, int? serviceId, string? targetAudience, int? createdBy)
    {
        var n = new Notification
        {
            Title = title, Message = message, CategoryId = categoryId, Priority = priority,
            CompanyId = companyId, SiteId = siteId, ServiceId = serviceId,
            TargetAudience = targetAudience, Status = "Active", IsActive = true,
            CreatedBy = createdBy, ModifiedBy = createdBy,
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        n._domainEvents.Add(new NotificationCreatedEvent(0, title, priority, categoryId));
        return n;
    }

    public void MarkRead(int userId)
    {
        Status = "Read"; ModifiedDate = DateTime.UtcNow; ModifiedBy = userId;
        _domainEvents.Add(new NotificationReadEvent(NotificationId, userId));
    }

    public void Archive(int? modifiedBy)
    {
        Status = "Archived"; IsActive = false; ModifiedDate = DateTime.UtcNow; ModifiedBy = modifiedBy;
        _domainEvents.Add(new NotificationArchivedEvent(NotificationId));
    }
}

public class NotificationCategory
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public int? Priority { get; set; } = 5;
    public int? DisplayOrder { get; set; } = 999;

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
