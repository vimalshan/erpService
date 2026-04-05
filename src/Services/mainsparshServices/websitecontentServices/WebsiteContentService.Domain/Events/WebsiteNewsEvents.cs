namespace WebsiteContentService.Domain.Events;

using WebsiteContentService.Domain.Common;

public class WebsiteNewsCreatedEvent(Guid aggregateId, string newsTitle, string? newsCategory, long createdBy)
    : DomainEvent(aggregateId)
{
    public string NewsTitle { get; } = newsTitle;
    public string? NewsCategory { get; } = newsCategory;
    public long CreatedBy { get; } = createdBy;
}

public class WebsiteNewsUpdatedEvent(Guid aggregateId, string newsTitle, long updatedBy)
    : DomainEvent(aggregateId)
{
    public string NewsTitle { get; } = newsTitle;
    public long UpdatedBy { get; } = updatedBy;
}

public class WebsiteNewsPublishedEvent(Guid aggregateId, string newsTitle, long publishedBy)
    : DomainEvent(aggregateId)
{
    public string NewsTitle { get; } = newsTitle;
    public long PublishedBy { get; } = publishedBy;
}
