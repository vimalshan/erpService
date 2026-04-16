namespace ActionService.Domain.Entities;

public class ActionItem
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public bool HighPriority { get; set; }
    public string? Message { get; set; }
    public string? Language { get; set; }
    public string? Service { get; set; }
    public string? Site { get; set; }
    public string? EntityType { get; set; }
    public int? EntityId { get; set; }
    public string? Subject { get; set; }
    public string? SnowLink { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    public static ActionItem Create(string action, DateTime? dueDate, bool highPriority, string? message,
        string? language, string? service, string? site, string? entityType, int? entityId, string? subject, string? snowLink)
    {
        var item = new ActionItem
        {
            Action = action,
            DueDate = dueDate,
            HighPriority = highPriority,
            Message = message,
            Language = language,
            Service = service,
            Site = site,
            EntityType = entityType,
            EntityId = entityId,
            Subject = subject,
            SnowLink = snowLink
        };
        item.AddDomainEvent(new ActionCreatedEvent(item));
        return item;
    }

    public void MarkComplete()
    {
        AddDomainEvent(new ActionCompletedEvent(this));
    }
}
